using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : SingletonBehavior<PlacementManager> {

    [Header("Internal")]
    [SerializeField] private Transform destroyDragViewer;
    [SerializeField] private Transform itemsParent;

    public bool isInPlaceMode { get; private set; }
    private bool isDragging;
    private Drag drag;
    private BuildingType selectedBlueprintType;
    private BlueprintBehavior selectedBlueprint;

    private List<BlueprintBehavior> blueprintsPreview = new List<BlueprintBehavior>();
    public Queue<BlueprintBehavior> blueprintsPlaced = new Queue<BlueprintBehavior>();
    public Queue<BuildingBehavior> itemsToDestroy = new Queue<BuildingBehavior>();

    private new void Awake() {
        base.Awake();
    }

    private void Start() {
        SubscribeToDragEvents();
    }

    private void SubscribeToDragEvents() {
        MouseInputManager.Instance.OnDragStart += OnDragStart;
        MouseInputManager.Instance.OnDragEnd += OnDragEnd;
    }

    public void StartPlaceMode(BuildingType itemType) {
        this.selectedBlueprintType = itemType;
        ResetSelectedItem();
        isInPlaceMode = true;
        CameraZoomSetEnabled(false);
    }

    public void EndPlaceMode() {
        if (isInPlaceMode) {
            selectedBlueprint.gameObject.SetActive(false);
            isInPlaceMode = false;
            CameraZoomSetEnabled(true);
            CancelDrag();
        }
    }

    private void ResetSelectedItem() {
        this.selectedBlueprint = NewBlueprintOfSelectedType();
        this.selectedBlueprint.transform.forward = Vector3.forward;
    }

    private BlueprintBehavior NewBlueprintOfSelectedType() {
        PoolBehavior pool = BuildingPoolManager.Instance.GetBlueprintPool(selectedBlueprintType);
        return pool.GetNext(itemsParent).GetComponent<BlueprintBehavior>();
    }

    private void CameraZoomSetEnabled(bool enabled) {
        Camera.main.GetComponent<CameraZoomBehavior>().enabled = enabled;
    }

    private void CancelDrag() {
        DisableBlueprintsOnPreview();
    }

    private void DisableBlueprintsOnPreview() {
        foreach (BlueprintBehavior blueprint in blueprintsPreview) {
            blueprint.gameObject.SetActive(false);
        }
        blueprintsPreview.Clear();
    }

    private void Update() {
        UpdateDrag();
        DrawDestroyDragViewer();

        if (!isInPlaceMode) return;

        UpdateSelectedBlueprint();
        UpdateBlueprintsPreview();
    }

    private void UpdateSelectedBlueprint() {
        UpdateSelectedBlueprintPosition();
        UpdateSelectedBlueprintRotation();
        UpdateSelectedBlueprintMaterial();
    }

    private void UpdateSelectedBlueprintPosition() {
        if (isDragging) return;

        Vector3 position = MouseInputManager.Instance.GetMouseWorldPosition();
        position = GridSystemManager.Instance.GetPositionSnappedToGrid(position, selectedBlueprint.GetSize());
        selectedBlueprint.transform.position = position;
    }

    private void UpdateSelectedBlueprintRotation() {
        UpdateRotationOnScroll();
        UpdateRotationOnDrag();
    }

    private void UpdateRotationOnScroll() {
        if (isDragging) return;
        if (!selectedBlueprint.IsRotable()) return;

        float scroll = MouseInputManager.Instance.scroll;
        if (scroll == 0) return;

        float scrollDir = scroll > 0 ? -1 : 1;
        selectedBlueprint.transform.Rotate(Vector3.up, scrollDir * 90);
    }

    private void UpdateRotationOnDrag() {
        if (!isDragging) return;
        if (!selectedBlueprint.IsRotable()) return;

        selectedBlueprint.transform.forward = drag.GetPrimaryAxisDirection();
    }

    private void UpdateSelectedBlueprintMaterial() {
        selectedBlueprint.UpdatePreview();
    }

    private void UpdateDrag() {
        drag = MouseInputManager.Instance.GetLatestDrag();
        if (drag == null) return;

        isDragging = !drag.IsEnded();
    }

    private void UpdateBlueprintsPreview() {
        if (!isDragging) return;

        DisableBlueprintsOnPreview();
        ShowBlueprintsPreview();
    }
    private void ShowBlueprintsPreview() {
        int blueprintsCount = GetBlueprintsNeededCount();
        float biggerSize = GetSelectedItemBiggerSize();
        Vector3 distancePerItem = drag.GetPrimaryAxisDirection() * biggerSize;

        for (int i = 1; i < blueprintsCount; i++) {
            Vector3 currentPosition = selectedBlueprint.transform.position + (distancePerItem * i);
            blueprintsPreview.Add(NewBlueprintForCurrentDrag(currentPosition));
        }
    }

    private int GetBlueprintsNeededCount() {
        Vector3 dragLatestPos = drag.GetLatestPosition();
        float dragLengthX = Mathf.Abs(drag.initialPos.x - dragLatestPos.x);
        float dragLengthZ = Mathf.Abs(drag.initialPos.z - dragLatestPos.z);
        float itemSizeX = selectedBlueprint.GetSize().x;
        float itemSizeY = selectedBlueprint.GetSize().z;

        if (dragLengthX > dragLengthZ) {
            return Mathf.CeilToInt(dragLengthX / itemSizeX);
        } else {
            return Mathf.CeilToInt(dragLengthZ / itemSizeY);
        }
    }

    private float GetSelectedItemBiggerSize() {
        float x = selectedBlueprint.GetSize().x;
        float z = selectedBlueprint.GetSize().z;
        return x > z ? x : z;
    }

    private BlueprintBehavior NewBlueprintForCurrentDrag(Vector3 position) {
        BlueprintBehavior blueprint = NewBlueprintOfSelectedType();
        blueprint.transform.forward = blueprint.IsRotable() ? drag.GetPrimaryAxisDirection() : blueprint.transform.forward;
        blueprint.transform.position = position;
        blueprint.UpdatePreview();

        return blueprint;
    }

    private void DrawDestroyDragViewer() {
        if (drag == null) return;

        bool isDraggingToDestroy = drag.mouseButton == Drag.MOUSE_BUTTON_RIGHT;
        if (isDraggingToDestroy) {
            ResizeDestroyDragViewer(drag.initialPos, drag.GetLatestPosition());
        }
    }

    private void ResizeDestroyDragViewer(Vector3 start, Vector3 end) {
        destroyDragViewer.position = GetBoundingBoxCenter(start, end);
        destroyDragViewer.localScale = GetBoundingBoxSize(start, end);
    }

    private void OnDragStart(Drag newDrag) {
        drag = newDrag;
        bool isDestroyDrag = drag.mouseButton == Drag.MOUSE_BUTTON_RIGHT;
        if (isDestroyDrag) {
            OnDragToDestroyStart();
        }
    }

    private void OnDragEnd() {
        bool isBuildDrag = drag.mouseButton == Drag.MOUSE_BUTTON_LEFT;
        bool isDestroyDrag = drag.mouseButton == Drag.MOUSE_BUTTON_RIGHT;
        if (isBuildDrag) {
            OnEndBuildDrag();
        } else if (isDestroyDrag) {
            OnDragToDestroyEnd();
        }
    }

    private void OnEndBuildDrag() {
        if (!isInPlaceMode) return;

        PlaceOrRotateBlueprint(selectedBlueprint);
        PlaceBlueprintsPreview();
        ResetSelectedItem();
    }

    private void PlaceBlueprintsPreview() {
        foreach (BlueprintBehavior blueprint in blueprintsPreview) {
            PlaceBlueprint(blueprint);
        }
        blueprintsPreview.Clear();
    }

    private void PlaceOrRotateBlueprint(BlueprintBehavior blueprint) {
        //Implement logic to rotate first one if putting over one of the same type.
        PlaceBlueprint(blueprint);
    }

    private void PlaceBlueprint(BlueprintBehavior blueprint) {
        bool placed = blueprint.TryPlaceHere();
        blueprint.SetReadyToBuild(placed);
        if (placed) {
            blueprintsPlaced.Enqueue(blueprint);
        } else {
            blueprint.gameObject.SetActive(false);
        }
    }

    private void OnDragToDestroyStart() {
        if (isInPlaceMode) return;

        destroyDragViewer.gameObject.SetActive(true);
    }

    private void OnDragToDestroyEnd() {
        bool isValidDrag = destroyDragViewer.gameObject.activeSelf;
        if (!isValidDrag) return;

        destroyDragViewer.gameObject.SetActive(false);
        DestroyItemsInside(drag.initialPos, drag.GetLatestPosition());
    }

    private void DestroyItemsInside(Vector3 start, Vector3 end) {
        Vector3 center = GetBoundingBoxCenter(start, end);
        Vector3 size = GetBoundingBoxSize(start, end);
        Collider[] hitColliders = Physics.OverlapBox(center, size / 2, Quaternion.identity);

        foreach (Collider collider in hitColliders) {
            BuildingBehavior building = collider.transform.GetComponent<BuildingBehavior>();
            if (building) {
                itemsToDestroy.Enqueue(building);
            }
        }
    }

    private Vector3 GetBoundingBoxCenter(Vector3 start, Vector3 end) {
        return new Vector3((start.x + end.x) / 2, 0, (start.z + end.z) / 2);
    }

    private Vector3 GetBoundingBoxSize(Vector3 start, Vector3 end) {
        return new Vector3(Mathf.Abs(end.x - start.x), 3f, Mathf.Abs(end.z - start.z));
    }

}
