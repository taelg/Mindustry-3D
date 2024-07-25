using UnityEngine;

public class OreBehavior : MonoBehaviour {

    [Header("Configurable")]
    [SerializeField] private OreType oreType = OreType.NONE;
    [SerializeField] private int oreTier = 1;

    private void OnTriggerEnter(Collider other) {
        TriggerOnTouchOreCheckers(other);
    }

    private void TriggerOnTouchOreCheckers(Collider other) {
        OreCheckerBehavior oreChecker = other.GetComponent<OreCheckerBehavior>();
        if (oreChecker)
            oreChecker.OnTouch(oreType, oreTier);
    }

    public OreType GetOreType() {
        return oreType;
    }


}
