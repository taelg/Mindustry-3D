using UnityEngine;

public class OreCheckerBehavior : MonoBehaviour {

    private bool isOre = false;
    private OreType oreType = OreType.NONE;

    public bool IsTouchingOre() {
        return isOre;
    }

    public OreType GetOreType() {
        return oreType;
    }

    public void OnTouch(OreType oreType) {
        isOre = true;
        this.oreType = oreType;

    }

}
