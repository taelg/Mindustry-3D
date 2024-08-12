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
        UpdateButton(button);
        UpdatePlaceMode();
    }

    private void InitializeButtonActions() {
        menuButtons.ToList().ForEach(button => button.AddEventOnClick(() => OnClickSelectBuilding(button)));
    }

    public void UnselectAllButtons() {
        activeButton = null;
        menuButtons.ToList().ForEach(button => button.Unselect());
    }

    private void UpdateButton(BuildingButtonBehavior button) {
        bool select = activeButton != button;
        button.SetSelect(select);
        activeButton = select ? button : null;
    }

    private void UpdatePlaceMode() {
        if (activeButton) {
            PlacementManager.Instance.StartPlaceMode(activeButton.GetBuildingType());
        } else {
            PlacementManager.Instance.EndPlaceMode();
        }
    }

}
