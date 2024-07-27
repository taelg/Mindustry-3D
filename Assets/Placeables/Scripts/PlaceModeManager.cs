using System.Collections.Generic;
using UnityEngine;

public class PlaceModeManager : SingletonBehavior<PlaceModeManager> {
    [SerializeField] private Camera activeCamera;
    [SerializeField] private Transform placeableParent;
    [SerializeField] private Grid referenceGrid;
    [SerializeField] private Dictionary<Vector2Int, bool> occupiedGrid = new Dictionary<Vector2Int, bool>();

    private bool isActive = false;
    private GameObject placeableSelected;

    private new void Awake() {
        base.Awake();
    }

    private void Update() {
        UpdateItemPosition();
        OnScrollRotateItem();
        OnClickAddItem();
    }

    public void StartMode(GameObject placeableSelected) {
        this.placeableSelected = placeableSelected;
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

    private void UpdateItemPosition() {
        if (isActive) {
            Vector3 position = RaycastUtils.GetMouseWorldPosition(activeCamera);
            BoxCollider boxCollider = placeableSelected.GetComponent<BoxCollider>();
            position = GetPositionSnappedToGrid(position, boxCollider.size);
            position = GetFixedHeight(position, 0);
            placeableSelected.transform.position = position;
        }
    }

    private Vector3 GetFixedHeight(Vector3 pos, int height) {
        return new Vector3(pos.x, height, pos.z);
    }

    private Vector3 GetPositionSnappedToGrid(Vector3 position, Vector3 size) {
        float gridCellSize = 1;
        Vector3 halfSize = size * 0.5f;
        float x = Mathf.Round((position.x - halfSize.x) / gridCellSize) * gridCellSize + halfSize.x;
        float z = Mathf.Round((position.z - halfSize.z) / gridCellSize) * gridCellSize + halfSize.z;
        Vector3 snappedPosition = new Vector3(x, position.y, z);
        return snappedPosition;
    }

    private void OnScrollRotateItem() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (isActive && scroll != 0) {
            scroll = scroll > 0 ? 1 : -1;
            placeableSelected.transform.Rotate(Vector3.up, scroll * 90);
        }
    }

    private void OnClickAddItem() {
        if (isActive) {
            bool clickToAdd = Input.GetMouseButtonDown(0);
            bool notInMenu = !RaycastUtils.IsMouseOverUI();
            bool isFreeSpace = IsGridEmptyAtCurrentPos();

            if (clickToAdd && notInMenu && isFreeSpace) {
                GameObject gameObject = Instantiate(placeableSelected, placeableSelected.transform.position, placeableSelected.transform.rotation, placeableParent);
                BoxCollider boxCollider = gameObject.transform.GetComponent<BoxCollider>();
                TakeSpace(gameObject.transform, boxCollider);
            }
        }
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

    private bool IsGridEmptyAtCurrentPos() {
        Vector3 position = placeableSelected.transform.position;
        BoxCollider boxCollider = placeableSelected.GetComponent<BoxCollider>();
        Vector3 size = boxCollider.size;
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
