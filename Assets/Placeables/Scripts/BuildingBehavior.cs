using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public abstract class BuildingBehavior : MonoBehaviour {

    [SerializeField] protected BuildingType type;
    [SerializeField] protected BoxCollider boxCollider;

    public abstract void OnBuild();

    public Vector3 GetSize() {
        if (!boxCollider)
            boxCollider = this.transform.GetComponent<BoxCollider>();

        return boxCollider.size;
    }

    public BuildingType GetBuildingType() {
        return type;
    }

    protected List<Tile> GetOwnTiles() {
        return GridSystemManager.Instance.GetOwnTiles(this);
    }

}