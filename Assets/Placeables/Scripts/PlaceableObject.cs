using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingObject", menuName = "ScriptableObjects/BuildingObject")]
public class BuildingObject : ScriptableObject {

    public Sprite icon;
    public GameObject blueprint;
    public GameObject building;
    public Cost[] materialsCost;

}

[Serializable]
public class Cost {
    public OreType ore;
    public int count;
}
