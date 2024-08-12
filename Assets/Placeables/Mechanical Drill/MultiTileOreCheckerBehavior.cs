using UnityEngine;

public class MultiTileOreCheckerBehavior : MonoBehaviour {

    [Header("Internal")]
    [SerializeField] private OreCheckerBehavior[] oreCheckers;

    private OreType oreType = OreType.NONE;
    private int oreTier = 1;
    private int effectiveTileCount = 0;

    public void Recalculate() {
        CalculateOreTier();
        CalculateOreType();
        CalculateEffectiveTileCount();
    }

    private void CalculateOreTier() {
        foreach (OreCheckerBehavior checker in oreCheckers) {
            if (checker.IsTouchingOre()) {
                oreTier = checker.GetOreTier();
                return;
            }
        }
    }

    private void CalculateOreType() {
        foreach (OreCheckerBehavior checker in oreCheckers) {
            if (checker.IsTouchingOre()) {
                oreType = checker.GetOreType();
                return;
            }
        }
    }

    private void CalculateEffectiveTileCount() {
        effectiveTileCount = 0;
        foreach (OreCheckerBehavior checker in oreCheckers) {
            if (checker.IsTouchingOre())
                effectiveTileCount += 1;
        }
    }

    public OreType GetOreType() {
        return oreType;
    }

    public int GetEffectiveTileCount() {
        return effectiveTileCount;
    }

    public float GetOreTier() {
        return oreTier;
    }



}
