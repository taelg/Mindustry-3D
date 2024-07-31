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