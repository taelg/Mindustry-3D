using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonBehavior<PoolManager> {

    [SerializeField] PlaceableTypePoolRelation[] poolsConfig;
    private Dictionary<PlaceableType, PoolBehavior> poolByType = new Dictionary<PlaceableType, PoolBehavior>();
    private Dictionary<PlaceableType, PoolBehavior> ghostPoolByType = new Dictionary<PlaceableType, PoolBehavior>();

    public PoolBehavior GetPoolByType(PlaceableType type) {
        return poolByType[type];
    }

    public PoolBehavior GetBlueprintPoolByType(PlaceableType type) {
        return ghostPoolByType[type];
    }

    private new void Awake() {
        base.Awake();
        LoadPools();
    }

    private void LoadPools() {
        foreach (var poolConfig in poolsConfig) {
            LoadPoolConfig(poolConfig);
        }
    }

    private void LoadPoolConfig(PlaceableTypePoolRelation poolConfig) {
        PlaceableType type = poolConfig.placeableType;
        poolByType.Add(type, poolConfig.poolBehavior);
        ghostPoolByType.Add(type, poolConfig.ghostPoolBehavior);
    }

}

[Serializable]
public class PlaceableTypePoolRelation {
    public PlaceableType placeableType;
    public PoolBehavior poolBehavior;
    public PoolBehavior ghostPoolBehavior;

}
