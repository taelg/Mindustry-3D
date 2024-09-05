using System.Linq;
using UnityEngine;

public class SelectMenuBehavior : MonoBehaviour {

    [SerializeField] private BuildingButtonBehavior[] menuButtons;

    private BuildingButtonBehavior activeButton = null;

    private void Start() {
        InitializeButtonActions();
    }

    private void OnDisable() {
        ResetMenu();
    }

    public void ResetMenu() {
        activeButton = null;
        UnselectAllButtons();
        UpdatePlaceMode();
        this.gameObject.SetActive(false);
    }


    private void OnClickSelectBuilding(BuildingButtonBehavior button) {
        UnselectAllButtons();
        UpdateActiveButton(button);
        UpdatePlaceMode();
    }

    private void InitializeButtonActions() {
        menuButtons.ToList().ForEach(button => button.AddEventOnClick(() => OnClickSelectBuilding(button)));
    }

    public void UnselectAllButtons() {
        menuButtons.ToList().ForEach(button => button.Unselect());
    }

    private void UpdateActiveButton(BuildingButtonBehavior button) {
        bool isSelecting = activeButton != button;
        button.SetSelect(isSelecting);
        activeButton = isSelecting ? button : null;
    }

    private void UpdatePlaceMode() {
        if (activeButton) {
            PlacementManager.Instance.StartPlaceMode(activeButton.GetBuildingType());
        } else {
            PlacementManager.Instance.EndPlaceMode();
        }
    }

}
