using UnityEngine;

public class PlaceModeManager : SingletonBehavior<PlaceModeManager> {
    [SerializeField] private Camera activeCamera;
    [SerializeField] private Transform placeableParent;

    private bool isActive = false;
    private GameObject selectedItem;

    private new void Awake() {
        base.Awake();
    }

    private void Update() {
        UpdateItemPosition();
        OnClickAddItem();
    }

    public void StartMode(GameObject placeableSelected) {
        this.selectedItem = placeableSelected;
        isActive = true;
        placeableSelected.SetActive(true);
    }

    public void EndMode() {
        isActive = false;
        selectedItem.SetActive(false);
        selectedItem.transform.position = Vector3.zero;
    }


    private void UpdateItemPosition() {
        if (isActive) {
            selectedItem.transform.position = RaycastUtils.GetMouseWorldPosition(activeCamera);
        }
    }

    private void OnClickAddItem() {
        bool clickToAdd = Input.GetMouseButtonDown(0);
        if (isActive && clickToAdd) {
            Instantiate(selectedItem, selectedItem.transform.position, selectedItem.transform.rotation, placeableParent);
        }
    }




}
