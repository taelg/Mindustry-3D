using System;
using System.Linq;
using UnityEngine;

public class TabMenuBehavior : MonoBehaviour {

    [SerializeField] private ButtonPanelRelation[] menuOptions;

    private GameObject activePanel = null;

    private void Start() {
        InitializeButtonActions();
    }

    private void OnClick(ButtonPanelRelation option) {
        CloseAllPanels();
        TogglePanel(option.panel);
        UnselectAllButtons();
        ToggleButton(option.button);
    }

    private void InitializeButtonActions() {
        menuOptions.ToList().ForEach(option => option.button.AddEventOnClick(() => OnClick(option)));
    }

    private void CloseAllPanels() {
        menuOptions.ToList().ForEach(option => option.panel.SetActive(false));
    }

    private void TogglePanel(GameObject panel) {
        bool isActiving = panel != activePanel;
        panel.SetActive(isActiving);
        activePanel = isActiving ? panel : null;
    }

    private void UnselectAllButtons() {
        menuOptions.ToList().ForEach(option => option.button.Unselect());
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