using System;
using System.Collections.Generic;
using UnityEngine;

public class PlaceModeManager : SingletonBehavior<PlaceModeManager> {

    [SerializeField] private Transform placeableParent;
    private bool isActive = false;
    private PlaceableType placeableType;
    private PlaceableGhostBehavior mainPlaceableGhost;
    private List<PlaceableGhostBehavior> ghostPreviews = new List<PlaceableGhostBehavior>();
    private Vector3 dragInitialPos;
    private bool isDragging = false;

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
        UpdateMainPaceablePreview();
        HandleAdditionalPlaceables();
    }

    private void MovePlaceableToMouse() {
        if (isActive && !isDragging) {
            Vector3 position = GetDragCurrentPosition();
            position = GridSystemManager.Instance.GetPositionSnappedToGrid(position, mainPlaceableGhost.GetSize());
            mainPlaceableGhost.transform.position = position;
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
            int additionalCount = GetAdditionalPlaceablesNeeded();
            AddGhostPreview(additionalCount);
        }
    }

    private void DisableAllGhosts() {
        foreach (PlaceableGhostBehavior ghost in ghostPreviews) {
            ghost.gameObject.SetActive(false);
        }
        ghostPreviews.Clear();
    }

    private void AddGhostPreview(int count) {
        Vector3 distancePerPrefab = GetDragDirection() * mainPlaceableGhost.GetSize().x;
        Vector3 currentDistance = mainPlaceableGhost.transform.position;
        for (int i = 0; i < count; i++) {
            currentDistance += distancePerPrefab;
            GameObject ghostPrefab = PoolManager.Instance.GetPoolGhostByType(placeableType).GetNext();
            ghostPrefab.transform.SetParent(placeableParent);
            ghostPrefab.transform.position = currentDistance;
            PlaceableGhostBehavior ghostBehavior = ghostPrefab.GetComponent<PlaceableGhostBehavior>();
            ghostBehavior.UpdatePreview();
            ghostPreviews.Add(ghostBehavior);
        }
    }

    public int GetAdditionalPlaceablesNeeded() {
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

    private Vector3 GetDragCurrentPosition() {
        return RaycastUtils.GetMouseWorldPosition(Camera.main);
    }

    private void PlaceGhostPreviews() {
        foreach (PlaceableGhostBehavior ghost in ghostPreviews) {
            bool placed = ghost.TryPlace();
            ghost.SetReadyToBuild(placed);
        }

        ghostPreviews.Clear();
        bool mainPlaced = mainPlaceableGhost.TryPlace();
        mainPlaceableGhost.SetReadyToBuild(mainPlaced);

        this.mainPlaceableGhost = PoolManager.Instance.GetPoolGhostByType(placeableType).GetNext().GetComponent<PlaceableGhostBehavior>();
        this.mainPlaceableGhost.transform.SetParent(placeableParent);
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

}
