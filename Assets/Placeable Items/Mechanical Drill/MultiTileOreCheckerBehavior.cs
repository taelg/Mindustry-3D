using UnityEngine;

public class MultiTileOreCheckerBehavior : MonoBehaviour {

    [Header("Internal")]
    [SerializeField] private OreCheckerBehavior[] oreCheckers;

    private OreType oreType = OreType.NONE;
    private float effectiveTileCount = 0;

    public void Recalculate() {
        CalculateOreType();
        CalculateEffectiveTileCount();
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
                effectiveTileCount += 1f;
        }
    }

    public OreType GetOreType() {
        return oreType;
    }

    public float GetEffectiveTileCount() {
        return effectiveTileCount;
    }



}
