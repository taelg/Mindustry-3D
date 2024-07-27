using System.Linq;
using UnityEngine;

public class SelectMenuBehavior : MonoBehaviour {

    [SerializeField] private PlaceableButtonBehavior[] menuButtons;

    private PlaceableButtonBehavior activeButton = null;

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


    private void OnClickSelectPlaceable(PlaceableButtonBehavior button) {
        UnselectAllButtons();
        UpdateButton(button);
        UpdatePlaceMode();
    }

    private void InitializeButtonActions() {
        menuButtons.ToList().ForEach(button => button.AddEventOnClick(() => OnClickSelectPlaceable(button)));
    }

    private void UnselectAllButtons() {
        menuButtons.ToList().ForEach(button => button.Unselect());
    }

    private void UpdateButton(PlaceableButtonBehavior button) {
        bool select = activeButton != button;
        button.SetSelect(select);
        activeButton = select ? button : null;
    }

    private void UpdatePlaceMode() {
        if (activeButton) {
            PlaceModeManager.Instance.StartMode(activeButton.GetPlaceableItem());
        } else {
            PlaceModeManager.Instance.EndMode();
        }
    }

}
