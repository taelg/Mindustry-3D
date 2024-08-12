public class Tile {
    public BuildingBehavior building = null;

    public OreType ore = OreType.NONE;

    public Tile(BuildingBehavior building) {
        SetTile(building);
    }

    public void UpdateTile(BuildingBehavior building) {
        SetTile(building);
    }

    private void SetTile(BuildingBehavior building) {
        this.building = building;
    }

    public bool IsEmpty() {
        return building == null;
    }

    public void RemoveBuilding() {
        building = null;
    }

}