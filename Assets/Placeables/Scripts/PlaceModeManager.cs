using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlaceModeManager : SingletonBehavior<PlaceModeManager> { //TODO: Refactor to impove quality
    [SerializeField] private Transform placeableParent;
    [SerializeField] private Transform destroySelection;
    private bool isActive = false;
    private PlaceableType placeableType;
    private PlaceableGhostBehavior mainPlaceableGhost;
    private List<PlaceableGhostBehavior> ghostPreviews = new List<PlaceableGhostBehavior>();
    private Vector3 dragInitialPos;
    private Vector3 dragDestroyInitialPos;
    private bool isDragging = false;
    private bool isDestroyDragging = false;
    public Queue<PlaceableGhostBehavior> placedGhosts = new Queue<PlaceableGhostBehavior>();
    public Queue<PlaceableBehavior> placeablesToDestroy = new Queue<PlaceableBehavior>();

    private new void Awake() {
        base.Awake();
    }

    public void StartMode(PlaceableType placeableType) {
        this.placeableType = placeableType;
        this.mainPlaceableGhost = GetNewPlaceableByType(placeableType);
        isActive = true;
        mainPlaceableGhost.transform.SetParent(placeableParent);
        mainPlaceableGhost.gameObject.SetActive(true);
        Camera.main.GetComponent<CameraZoomBehavior>().enabled = false;
    }

    private PlaceableGhostBehavior GetNewPlaceableByType(PlaceableType placeableType) {
        PoolBehavior pool = PoolManager.Instance.GetPoolGhostByType(placeableType);
        GameObject placeable = pool.GetNext();
        return placeable.GetComponent<PlaceableGhostBehavior>();
    }

    public void EndMode() {
        if (isActive) {
            CancelDrag();
            mainPlaceableGhost.gameObject.SetActive(false);
            isActive = false;
            Camera.main.GetComponent<CameraZoomBehavior>().enabled = true;
        }
    }

    private void Update() {
        HandleInputs();
        MovePlaceableToMouse();
        UpdateDestroySelection();
        UpdateMainPaceablePreview();
        HandleAdditionalPlaceables();
    }

    private void MovePlaceableToMouse() {
        if (isActive && !isDragging) {
            Vector3 position = GetDragCurrentPosition();
            position = GridSystemManager.Instance.GetPositionSnappedToGrid(position, mainPlaceableGhost.GetSize());
            mainPlaceableGhost.transform.position = position;
        } else if (isActive && isDragging) {
            mainPlaceableGhost.transform.forward = GetDragDirection();
        }
    }

    private void UpdateMainPaceablePreview() {
        if (isActive && !isDragging) {
            mainPlaceableGhost.UpdatePreview();
        }
    }

    private void HandleAdditionalPlaceables() {
        if (isActive && isDragging) {
            DisableAllGhosts();
            int additionalCount = GetAdditionalPlaceablesNeededCount();
            ShowGhostsPreview(additionalCount);
        }
    }

    private void DisableAllGhosts() {
        foreach (PlaceableGhostBehavior ghost in ghostPreviews) {
            ghost.gameObject.SetActive(false);
        }
        ghostPreviews.Clear();
    }

    private void ShowGhostsPreview(int count) {
        Vector3 distancePerPrefab = GetDragDirection() * mainPlaceableGhost.GetSize().x;
        Vector3 currentDistance = mainPlaceableGhost.transform.position;
        for (int i = 0; i < count; i++) {
            currentDistance += distancePerPrefab;
            GameObject ghostPrefab = PoolManager.Instance.GetPoolGhostByType(placeableType).GetNext();
            ghostPrefab.transform.SetParent(placeableParent);
            ghostPrefab.transform.position = currentDistance;
            ghostPrefab.transform.forward = GetDragDirection();
            PlaceableGhostBehavior ghostBehavior = ghostPrefab.GetComponent<PlaceableGhostBehavior>();
            ghostBehavior.UpdatePreview();
            ghostPreviews.Add(ghostBehavior);
        }
    }

    public int GetAdditionalPlaceablesNeededCount() {
        Vector3 dragPosition = GetDragCurrentPosition();
        float dragLenghtX = Mathf.Abs(dragInitialPos.x - dragPosition.x);
        float dragLenghtZ = Mathf.Abs(dragInitialPos.z - dragPosition.z);

        int additionalCount = 0;
        if (dragLenghtX > dragLenghtZ) {
            additionalCount = (int)Mathf.Round(dragLenghtX / mainPlaceableGhost.GetSize().x);
        } else {
            additionalCount = (int)Mathf.Round(dragLenghtZ / mainPlaceableGhost.GetSize().z);
        }

        return additionalCount;
    }

    private void HandleInputs() {
        if (isActive) {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
                OnScrollRotatePlaceable(scroll);

            if (Input.GetMouseButtonDown(0) && !RaycastUtils.IsMouseOverUI())
                StartDrag();

            if (Input.GetMouseButtonUp(0))
                EndDrag();
        }
        if (Input.GetMouseButtonDown(1) && !RaycastUtils.IsMouseOverUI())
            StartDestroyDrag();
        if (Input.GetMouseButtonUp(1))
            EndDestroyDrag();
    }

    private void OnScrollRotatePlaceable(float scroll) {
        float scrollDir = scroll > 0 ? -1 : 1;
        mainPlaceableGhost.transform.Rotate(Vector3.up, scrollDir * 90);
    }

    private void StartDrag() {
        isDragging = true;
        dragInitialPos = mainPlaceableGhost.transform.position;
    }

    private void EndDrag() {
        if (isDragging) {
            isDragging = false;
            PlaceGhostPreviews();
            DisableAllGhosts();
        }
    }

    private void CancelDrag() {
        if (isDragging) {
            isDragging = false;
            DisableAllGhosts();
        }
    }

    private void StartDestroyDrag() {
        isDestroyDragging = true;
        dragDestroyInitialPos = GetDragCurrentPosition();
    }

    private void EndDestroyDrag() {
        isDestroyDragging = false;
        Vector3 dragDestroyFinalPos = GetDragCurrentPosition();
        DestroyObjectsInside(dragDestroyInitialPos, dragDestroyFinalPos);
    }

    private Vector3 GetDragCurrentPosition() {
        return RaycastUtils.GetMouseWorldPosition(Camera.main);
    }

    private void PlaceGhostPreviews() {
        PlaceGhost(mainPlaceableGhost);
        foreach (PlaceableGhostBehavior ghost in ghostPreviews) {
            PlaceGhost(ghost);
        }
        ghostPreviews.Clear();

        this.mainPlaceableGhost = PoolManager.Instance.GetPoolGhostByType(placeableType).GetNext().GetComponent<PlaceableGhostBehavior>();
        this.mainPlaceableGhost.transform.SetParent(placeableParent);
    }

    private void PlaceGhost(PlaceableGhostBehavior ghost) {
        bool placed = ghost.TryPlace();
        ghost.SetReadyToBuild(placed);
        if (placed) {
            placedGhosts.Enqueue(ghost);
        } else {
            ghost.gameObject.SetActive(false);
        }
    }

    private Vector3 GetDragDirection() {
        Vector3 currentPos = GetDragCurrentPosition();
        Vector3 direction = currentPos - dragInitialPos;
        float distanceX = Mathf.Abs(direction.x);
        float distanceZ = Mathf.Abs(direction.z);

        if (distanceX > distanceZ) {
            direction = new Vector3(Mathf.Sign(direction.x), 0, 0);
        } else {
            direction = new Vector3(0, 0, Mathf.Sign(direction.z));
        }
        return direction;
    }

    private void DestroyObjectsInside(Vector3 start, Vector3 end) {
        start = new Vector3(start.x, 0, start.z);
        end = new Vector3(end.x, 0, end.z);
        Vector3 center = (start + end) / 2;
        Vector3 size = new Vector3(Mathf.Abs(end.x - start.x), 2.5f, Mathf.Abs(end.z - start.z));
        Collider[] hitColliders = Physics.OverlapBox(center, size / 2, Quaternion.identity);

        foreach (Collider collider in hitColliders) {
            PlaceableBehavior placeable = collider.transform.GetComponent<PlaceableBehavior>();
            if (placeable) {
                placeablesToDestroy.Enqueue(placeable);
            }
        }
    }

    private void UpdateDestroySelection() {
        destroySelection.gameObject.SetActive(isDestroyDragging);
        if (isDestroyDragging) {
            Vector3 start = dragDestroyInitialPos;
            Vector3 end = GetDragCurrentPosition();
            ResizeDestroySelection(start, end);
        }
    }

    private void ResizeDestroySelection(Vector3 start, Vector3 end) {
        start = new Vector3(start.x, 0, start.z);
        end = new Vector3(end.x, 0, end.z);
        Vector3 center = (start + end) / 2;
        Vector3 size = new Vector3(Mathf.Abs(end.x - start.x), 2.5f, Mathf.Abs(end.z - start.z));

        destroySelection.transform.position = center;
        destroySelection.transform.localScale = size;
    }

}
