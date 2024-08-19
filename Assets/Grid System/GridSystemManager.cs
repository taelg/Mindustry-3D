using System.Collections.Generic;
using UnityEngine;

public class GridSystemManager : SingletonBehavior<GridSystemManager> {

    [SerializeField] private Terrain terrain;

    private readonly float GRID_CELL_SIZE = 1;
    private Dictionary<Vector2Int, Tile> grid = new Dictionary<Vector2Int, Tile>();

    private new void Awake() {
        base.Awake();
    }

    public Vector3 GetPositionSnappedToGrid(Vector3 position, Vector3 size) {
        Vector3 halfSize = size * 0.5f;
        float x = Mathf.Round((position.x - halfSize.x) / GRID_CELL_SIZE) * GRID_CELL_SIZE + halfSize.x;
        float z = Mathf.Round((position.z - halfSize.z) / GRID_CELL_SIZE) * GRID_CELL_SIZE + halfSize.z;
        Vector3 snappedPosition = new Vector3(x, 0, z);
        return snappedPosition;
    }

    public List<Tile> GetOwnTiles(BuildingBehavior building) {
        List<Tile> ownTiles = new List<Tile>();
        Vector3 size = building.GetSize();
        Vector3 bottomLeft = building.transform.position - size * 0.5f;
        for (int x = Mathf.FloorToInt(bottomLeft.x); x < Mathf.CeilToInt(bottomLeft.x + size.x); x++) {
            for (int z = Mathf.FloorToInt(bottomLeft.z); z < Mathf.CeilToInt(bottomLeft.z + size.z); z++) {
                Vector2Int tileKeyPos = new Vector2Int(x, z);
                bool tileExist = grid.ContainsKey(tileKeyPos);
                Debug.Log("tileExist:" + tileExist);
                ownTiles.Add(tileExist ? grid[tileKeyPos] : AddEmptyTile(tileKeyPos));
            }
        }

        return ownTiles;
    }

    public bool IsGridEmpty(BuildingBehavior building) {
        Vector3 position = building.transform.position;
        Vector3 size = building.GetSize();
        Vector3 bottomLeft = position - size * 0.5f;

        for (int x = Mathf.FloorToInt(bottomLeft.x); x < Mathf.CeilToInt(bottomLeft.x + size.x); x++) {
            for (int z = Mathf.FloorToInt(bottomLeft.z); z < Mathf.CeilToInt(bottomLeft.z + size.z); z++) {
                Vector2Int tileKeyPos = new Vector2Int(x, z);
                bool existKey = grid.ContainsKey(tileKeyPos);
                if (existKey && !grid[tileKeyPos].IsEmpty()) {
                    return false;
                }
            }
        }

        return true;
    }

    public void TakeSpace(BuildingBehavior building) {
        Vector3 size = building.GetSize();
        Vector3 bottomLeft = building.transform.position - size * 0.5f;
        for (int x = Mathf.FloorToInt(bottomLeft.x); x < Mathf.CeilToInt(bottomLeft.x + size.x); x++) {
            for (int z = Mathf.FloorToInt(bottomLeft.z); z < Mathf.CeilToInt(bottomLeft.z + size.z); z++) {
                AddOrUpdateTileBuilding(x, z, building);
            }
        }
    }

    public void TakeSpace(OreBehavior oreVein) {
        Vector3 size = oreVein.GetBoxCollider().size;
        Vector3 bottomLeft = oreVein.transform.position - size * 0.5f;
        for (int x = Mathf.FloorToInt(bottomLeft.x); x < Mathf.CeilToInt(bottomLeft.x + size.x); x++) {
            for (int z = Mathf.FloorToInt(bottomLeft.z); z < Mathf.CeilToInt(bottomLeft.z + size.z); z++) {
                AddOrUpdateTileOre(x, z, oreVein);
            }
        }
    }

    public void LeaveSpace(BuildingBehavior building) {
        Vector3 size = building.GetSize();
        Vector3 bottomLeft = building.transform.position - size * 0.5f;
        for (int x = Mathf.FloorToInt(bottomLeft.x); x < Mathf.CeilToInt(bottomLeft.x + size.x); x++) {
            for (int z = Mathf.FloorToInt(bottomLeft.z); z < Mathf.CeilToInt(bottomLeft.z + size.z); z++) {
                AddOrUpdateTileBuilding(x, z, null);
            }
        }
    }

    private Tile AddEmptyTile(Vector2Int tileKeyPos) {
        Tile newTile = new Tile();
        grid.Add(tileKeyPos, newTile);
        return newTile;
    }

    private Tile AddOrUpdateTileBuilding(int tileX, int tileZ, BuildingBehavior newBuilding) {
        Vector2Int key = new Vector2Int(tileX, tileZ);
        if (grid.ContainsKey(key)) {
            grid[key].SetBuilding(newBuilding);
            return grid[key];
        } else {
            Tile newTile = new Tile().SetBuilding(newBuilding);
            grid.Add(key, newTile);
            return newTile;
        }
    }

    private Tile AddOrUpdateTileOre(int tileX, int tileZ, OreBehavior oreVein) {
        Vector2Int key = new Vector2Int(tileX, tileZ);
        if (grid.ContainsKey(key)) {
            grid[key].SetOreType(oreVein.GetOreType());
            return grid[key];
        } else {
            Tile newTile = new Tile().SetOreType(oreVein.GetOreType());
            grid.Add(key, newTile);
            return newTile;
        }
    }

}