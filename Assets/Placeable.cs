using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour {

    [SerializeField] private BoxCollider boxCollider;

    void Start() {
        TakePlaceOnGrid();
    }

    private void TakePlaceOnGrid() {
        PlaceModeManager.Instance.TakeSpace(this.transform, boxCollider);
    }

}
