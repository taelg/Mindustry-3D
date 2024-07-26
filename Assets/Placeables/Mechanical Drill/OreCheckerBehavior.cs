using UnityEngine;

public class OreCheckerBehavior : MonoBehaviour {

    private bool isOre = false;
    private int oreTier = 1;
    private OreType oreType = OreType.NONE;

    public bool IsTouchingOre() {
        return isOre;
    }

    public OreType GetOreType() {
        return oreType;
    }

    public int GetOreTier() {
        return oreTier;
    }

    public void OnTouch(OreType oreType, int oreTier) {
        isOre = true;
        this.oreType = oreType;
        this.oreTier = oreTier;

    }

}
