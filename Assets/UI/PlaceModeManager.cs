using System.Collections;
using UnityEngine;

public class PlaceModeManager : MonoBehaviour {
    [SerializeField] private Camera activeCamera;
    public static PlaceModeManager Instance;

    private bool isActive;
    private GameObject selectedItem;


    private void Start() {
        isActive = false;
        StartSingleton();
    }

    private void StartSingleton() {
        if (Instance != null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this.gameObject);
            Debug.LogError("You are trying to initialize multiple Singletons of type: " + this.gameObject.name);
        }
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
            GameObject newInstance = Instantiate(selectedItem);
            newInstance.transform.position =
        }
    }




}
