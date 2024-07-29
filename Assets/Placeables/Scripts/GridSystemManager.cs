using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemManager : SingletonBehavior<GridSystemManager> {
    private Dictionary<Vector2Int, Tile> occupiedGrid = new Dictionary<Vector2Int, Tile>();

    private new void Awake() {
        base.Awake();
    }

    public void Place(PlaceableGhostBehavior placeable) {
        placeable.TryPlace();
    }

    public Vector3 GetPositionSnappedToGrid(PlaceableGhostBehavior placeable) {
        Vector3 position = placeable.transform.position;
        Vector3 size = placeable.GetSize();
        return GetPositionSnappedToGrid(position, size);
    }

    public Vector3 GetPositionSnappedToGrid(Vector3 position, Vector3 size) {
        float gridCellSize = 1;
        Vector3 halfSize = size * 0.5f;
        float x = Mathf.Round((position.x - halfSize.x) / gridCellSize) * gridCellSize + halfSize.x;
        float z = Mathf.Round((position.z - halfSize.z) / gridCellSize) * gridCellSize + halfSize.z;
        Vector3 snappedPosition = new Vector3(x, 0, z);
        return snappedPosition;
    }

    public bool IsGridEmpty(PlaceableBehavior placeable) {
        Vector3 position = placeable.transform.position;
        Vector3 size = placeable.GetSize();
        Vector3 bottomLeft = position - size * 0.5f;

        for (int x = Mathf.FloorToInt(bottomLeft.x); x < Mathf.CeilToInt(bottomLeft.x + size.x); x++) {
            for (int z = Mathf.FloorToInt(bottomLeft.z); z < Mathf.CeilToInt(bottomLeft.z + size.z); z++) {
                Vector2Int tileKeyPos = new Vector2Int(x, z);
                bool existKey = occupiedGrid.ContainsKey(tileKeyPos);
                if (existKey && !occupiedGrid[tileKeyPos].IsEmpty()) {
                    return false;
                }
            }
        }

        return true;
    }

    public void TakeSpace(PlaceableBehavior placeable) {
        Vector3 size = placeable.GetSize();
        Vector3 bottomLeft = placeable.transform.position - size * 0.5f;
        for (int x = Mathf.FloorToInt(bottomLeft.x); x < Mathf.CeilToInt(bottomLeft.x + size.x); x++) {
            for (int z = Mathf.FloorToInt(bottomLeft.z); z < Mathf.CeilToInt(bottomLeft.z + size.z); z++) {
                AddOrUpdateTileGrid(x, z, placeable);
            }
        }
    }

    private void AddOrUpdateTileGrid(int x, int z, PlaceableBehavior placeable) {
        Vector2Int key = new Vector2Int(x, z);
        if (occupiedGrid.ContainsKey(key)) {
            occupiedGrid[key].UpdateTile(placeable);
        } else {
            occupiedGrid.Add(key, new Tile(placeable));
        }
    }

}


public class Tile {
    public PlaceableBehavior placeable = null;

    public OreType ore = OreType.NONE;

    public Tile(PlaceableBehavior placeable) {
        SetTile(placeable);
    }

    public void UpdateTile(PlaceableBehavior placeable) {
        SetTile(placeable);
    }

    private void SetTile(PlaceableBehavior placeable) {
        this.placeable = placeable;
    }

    public bool IsEmpty() {
        return placeable == null;
    }

    public void RemovePlaceable() {
        placeable = null;
    }

}