using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalDrillBlueprint : BlueprintBehavior {

    private int effectiveOreTileCount = 0;
    private OreType foundOreType = OreType.NONE;
    private int foundOreTier = 1;


    public override void UpdatePreview() {
        CalculateOreInformation();
        bool isEnoughtSpace = GridSystemManager.Instance.IsGridEmpty(this);
        bool isPlacedOnOres = effectiveOreTileCount > 0;
        canPlaceHere = isEnoughtSpace && isPlacedOnOres;
        UpdateMaterial();
    }

    private void CalculateOreInformation() {
        List<Tile> ownTiles = GetOwnTiles();
        effectiveOreTileCount = 0;

        foreach (Tile tile in ownTiles) {
            OreType foundOreType = tile.oreType;
            if (foundOreType == OreType.NONE)
                continue;

            bool noOreFoundYet = this.foundOreType == OreType.NONE;
            if (noOreFoundYet) {
                this.foundOreType = foundOreType;
                this.foundOreTier = 1; //TODO: Implement Ore Tiers.
            }
            effectiveOreTileCount++;
        }
    }

}
