using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPoolManager : SingletonBehavior<BuildingPoolManager> {

    [SerializeField] BuildingTypePoolRelation[] poolsConfig;
    private Dictionary<BuildingType, PoolBehavior> buildingsPool = new Dictionary<BuildingType, PoolBehavior>();
    private Dictionary<BuildingType, PoolBehavior> blueprintsPool = new Dictionary<BuildingType, PoolBehavior>();

    public PoolBehavior GetBuildingPool(BuildingType type) {
        return buildingsPool[type];
    }

    public PoolBehavior GetBlueprintPool(BuildingType type) {
        return blueprintsPool[type];
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

    private void LoadPoolConfig(BuildingTypePoolRelation poolConfig) {
        BuildingType type = poolConfig.type;
        buildingsPool.Add(type, poolConfig.buildingPool);
        blueprintsPool.Add(type, poolConfig.blueprintPool);
    }

}

[Serializable]
public class BuildingTypePoolRelation {
    public BuildingType type;
    public PoolBehavior buildingPool;
    public PoolBehavior blueprintPool;

}
