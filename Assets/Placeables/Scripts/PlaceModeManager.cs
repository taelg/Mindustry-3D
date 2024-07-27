using System.Collections.Generic;
using UnityEngine;

public class PlaceModeManager : SingletonBehavior<PlaceModeManager> {
    [SerializeField] private Camera activeCamera;
    [SerializeField] private Transform placeableParent;
    [SerializeField] private Dictionary<Vector2Int, bool> occupiedGrid = new Dictionary<Vector2Int, bool>();

    private bool isActive = false;
    private GameObject placeableSelected;
    private BoxCollider placeableBoxCollider;
    private Vector3 dragInitialPos;
    private Vector3 dragFinalPos;
    private bool isDragging = false;

    private new void Awake() {
        base.Awake();
    }

    public void StartMode(GameObject placeableSelected) {
        this.placeableSelected = placeableSelected;
        this.placeableBoxCollider = placeableSelected.GetComponent<BoxCollider>();
        isActive = true;
        placeableSelected.SetActive(true);
        activeCamera.GetComponent<CameraZoomBehavior>().enabled = false;
    }

    public void EndMode() {
        if (isActive) {
            placeableSelected.transform.position = Vector3.zero;
            placeableSelected.SetActive(false);
            isActive = false;
            activeCamera.GetComponent<CameraZoomBehavior>().enabled = true;
        }
    }

    private void Update() {
        HandleInputs();
        MovePlaceableToMouseWorldPosition();
    }

    private void MovePlaceableToMouseWorldPosition() {
        if (isActive) {
            Vector3 position = RaycastUtils.GetMouseWorldPosition(activeCamera);
            position = GetPositionSnappedToGrid(position);
            placeableSelected.transform.position = position;
        }
    }

    private void HandleInputs() {
        if (isActive) {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
                OnScrollRotatePlaceable(scroll);

            if (Input.GetMouseButtonDown(0) && !RaycastUtils.IsMouseOverUI())
                OnStartDrag();

            if (Input.GetMouseButtonUp(0))
                OnEndDrag();
        }
    }

    private void OnScrollRotatePlaceable(float scroll) {
        float scrollDir = scroll > 0 ? -1 : 1;
        placeableSelected.transform.Rotate(Vector3.up, scrollDir * 90);
    }
    private void OnStartDrag() {
        isDragging = true;
        dragInitialPos = placeableSelected.transform.position;
    }

    private void OnEndDrag() {
        if (isDragging) {
            isDragging = false;
            dragFinalPos = placeableSelected.transform.position;
            OnEndDragAddPlaceableToWorld();
            Debug.DrawLine(dragInitialPos, dragFinalPos, Color.red, 5);
        }
    }

    private void OnEndDragAddPlaceableToWorld() {
        Vector3 drag = GetDrag();
        Vector3 placeableSize = placeableBoxCollider.size;
        Vector3 currentPos = dragInitialPos;
        int totalPlaceables = drag.x != 0
            ? Mathf.CeilToInt(Mathf.Abs(dragFinalPos.x - dragInitialPos.x) / placeableSize.x)
            : Mathf.CeilToInt(Mathf.Abs(dragFinalPos.z - dragInitialPos.z) / placeableSize.z);

        for (int i = 0; i <= totalPlaceables; i++) {
            if (IsGridEmptyAtCurrentPos(currentPos, placeableSize)) {
                GameObject gameObject = Instantiate(placeableSelected, currentPos, placeableSelected.transform.rotation, placeableParent);
                TakeSpace(gameObject.transform, placeableBoxCollider);
            }
            currentPos += drag * placeableSize.x;
        }

    }

    private Vector3 GetDrag() {
        Vector3 direction = dragFinalPos - dragInitialPos;
        float distanceX = Mathf.Abs(direction.x);
        float distanceZ = Mathf.Abs(direction.z);

        if (distanceX > distanceZ) {
            direction = new Vector3(Mathf.Sign(direction.x), 0, 0);
        } else {
            direction = new Vector3(0, 0, Mathf.Sign(direction.z));
        }
        return direction;
    }

    private Vector3 GetPositionSnappedToGrid(Vector3 position) {
        Vector3 size = placeableBoxCollider.size;
        float gridCellSize = 1;
        Vector3 halfSize = size * 0.5f;
        float x = Mathf.Round((position.x - halfSize.x) / gridCellSize) * gridCellSize + halfSize.x;
        float z = Mathf.Round((position.z - halfSize.z) / gridCellSize) * gridCellSize + halfSize.z;
        Vector3 snappedPosition = new Vector3(x, 0, z);
        return snappedPosition;
    }

    public void TakeSpace(Transform transform, BoxCollider boxCollider) {
        Vector3 size = boxCollider.size;
        Vector3 bottomLeft = transform.position - size * 0.5f;

        for (int x = Mathf.FloorToInt(bottomLeft.x); x < Mathf.CeilToInt(bottomLeft.x + size.x); x++) {
            for (int z = Mathf.FloorToInt(bottomLeft.z); z < Mathf.CeilToInt(bottomLeft.z + size.z); z++) {
                occupiedGrid.Add(new Vector2Int(x, z), true);
            }
        }
    }

    private bool IsGridEmptyAtCurrentPos(Vector3 position, Vector3 size) {
        Vector3 bottomLeft = position - size * 0.5f;

        for (int x = Mathf.FloorToInt(bottomLeft.x); x < Mathf.CeilToInt(bottomLeft.x + size.x); x++) {
            for (int z = Mathf.FloorToInt(bottomLeft.z); z < Mathf.CeilToInt(bottomLeft.z + size.z); z++) {
                Vector2Int cellPosition = new Vector2Int(x, z);
                bool existKey = occupiedGrid.ContainsKey(cellPosition);
                if (existKey && occupiedGrid[cellPosition]) {
                    return false;
                }
            }
        }
        return true;
    }

}
