<<<<<<< HEAD
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
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreBehavior : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        RandomizeRotation();

    }

    private void RandomizeRotation() {
        float rotationX = RandomUtils.RandomRotation();
        float rotationY = RandomUtils.RandomRotation();
        float rotationZ = RandomUtils.RandomRotation();
        this.transform.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
>>>>>>> b97cff57c6282d54b53a08d536f381b8798264a1
    }


}
