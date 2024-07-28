using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableButtonBehavior : MenuButtonBehavior {

    [SerializeField] private PlaceableType placeableType;

    public PlaceableType GetPlaceableType() {
        return placeableType;
    }

}
