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
        if (Input.GetMouseButtonDown(1)) {
            ResetMenu();
        }
    }

    private void ResetMenu() {
        activePanel = null;
        CloseAllPanels();
        UnselectAllButtons();
        ResetAllPanels();
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

    private void ResetAllPanels() {
        menuOptions.ToList().ForEach(option => option.panel.GetComponent<SelectMenuBehavior>().ResetMenu());
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