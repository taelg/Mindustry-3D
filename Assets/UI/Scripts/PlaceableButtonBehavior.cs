using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButtonBehavior : MenuButtonBehavior {

    [SerializeField] private BuildingType buildingType;

    public BuildingType GetBuildingType() {
        return buildingType;
    }

}
