using System;
using System.Linq;
using UnityEngine;

public class TabMenuBehavior : MonoBehaviour {

    [SerializeField] private ButtonPanelRelation[] menuOptions;

    private GameObject activePanel = null;

    private void Start() {
        InitializeButtonActions();
    }

    private void OnClickSelectPanel(ButtonPanelRelation option) {
        CloseAllPanels();
        TogglePanel(option.panel);
        UnselectAllButtons();
        ToggleButton(option.button);
    }

    private void Update() {
        if (MouseInputManager.Instance.rightButtonDown) {
            TurnOffPlaceMode();
        }
    }

    private void TurnOffPlaceMode() {
        PlacementManager.Instance.EndPlaceMode();
        UnselectAllPanelsButtons();
    }

    private void InitializeButtonActions() {
        menuOptions.ToList().ForEach(option => option.button.AddEventOnClick(() => OnClickSelectPanel(option)));
    }

    private void CloseAllPanels() {
        menuOptions.ToList().ForEach(option => option.panel.SetActive(false));
    }

    private void UnselectAllButtons() {
        menuOptions.ToList().ForEach(option => option.button.Unselect());
    }

    private void UnselectAllPanelsButtons() {
        menuOptions.ToList().ForEach(option => option.panel.GetComponent<SelectMenuBehavior>().UnselectAllButtons());
    }

    private void TogglePanel(GameObject panel) {
        bool isActiving = panel != activePanel;
        panel.SetActive(isActiving);
        activePanel = isActiving ? panel : null;
    }

    private void ToggleButton(MenuButtonBehavior button) {
        bool isSelected = activePanel;
        button.SetSelect(isSelected);
    }

}


[Serializable]
public class ButtonPanelRelation {
    public GameObject panel;
    public MenuButtonBehavior button;
}