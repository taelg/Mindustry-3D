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
    }


}
