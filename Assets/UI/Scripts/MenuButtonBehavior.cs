using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[Serializable]
public class MenuButtonBehavior : MonoBehaviour {
    [SerializeField] private GameObject placeableItem;
    [SerializeField] private GameObject selectedBorder;
    private Button button;

    private void Awake() {
        button = this.GetComponent<Button>();
    }

    public void Unselect() {
        selectedBorder.SetActive(false);
    }

    public void Select() {
        selectedBorder.SetActive(true);
    }

    public void SetSelect(bool isSelected) {
        selectedBorder.SetActive(isSelected);
    }

    public void AddEventOnClick(UnityAction action) {
        button.onClick.AddListener(action);
    }

    public GameObject GetPlaceableItem() {
        return placeableItem;
    }

}
