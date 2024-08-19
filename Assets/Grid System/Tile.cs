public class Tile {
    public BuildingBehavior building = null;

    public OreType oreType = OreType.NONE;

    public Tile() { }

    public Tile SetBuilding(BuildingBehavior building) {
        this.building = building;
        return this;
    }

    public Tile SetOreType(OreType oreType) {
        this.oreType = oreType;
        return this;
    }

    public bool IsEmpty() {
        return building == null;
    }

}