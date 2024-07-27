using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableButtonBehavior : MenuButtonBehavior {

    [SerializeField] private GameObject placeableItem;

    public GameObject GetPlaceableItem() {
        return placeableItem;
    }

}
