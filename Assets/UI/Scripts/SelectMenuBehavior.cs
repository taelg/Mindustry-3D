using System.Linq;
using UnityEngine;

public class SelectMenuBehavior : MonoBehaviour {

    [SerializeField] private MenuButtonBehavior[] menuButtons;

    private MenuButtonBehavior activeButton = null;

    private void Start() {
        InitializeButtonActions();
    }

    private void OnSelectButton(MenuButtonBehavior button) {
        UnselectAllButtons();
        UpdateButton(button);
        UpdatePlaceMode();
    }

    private void InitializeButtonActions() {
        menuButtons.ToList().ForEach(button => button.AddEventOnClick(() => OnSelectButton(button)));
    }

    private void UnselectAllButtons() {
        menuButtons.ToList().ForEach(button => button.Unselect());
    }

    private void UpdateButton(MenuButtonBehavior button) {
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
