using UnityEngine;

public class OreBehavior : MonoBehaviour {

    [Header("Configurable")]
    [SerializeField] private OreType oreType = OreType.NONE;

    private void OnTriggerEnter(Collider other) {
        TriggerOnTouchOreCheckers(other);
    }

    private void TriggerOnTouchOreCheckers(Collider other) {
        OreCheckerBehavior oreChecker = other.GetComponent<OreCheckerBehavior>();
        if (oreChecker)
            oreChecker.OnTouch(oreType);
    }

    public OreType GetOreType() {
        return oreType;
    }


}
