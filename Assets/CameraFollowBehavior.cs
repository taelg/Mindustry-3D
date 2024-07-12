using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBehavior : MonoBehaviour {

    [SerializeField] private Transform transformToFollow;

    void Update() {
        this.transform.position = transformToFollow.position;
    }
}
