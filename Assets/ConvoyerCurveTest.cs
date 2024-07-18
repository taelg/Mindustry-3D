using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoyerCurveTest : MonoBehaviour {

    [SerializeField] float speed = 100;
    [SerializeField] float startingRotation = 0;
    [SerializeField] Material material;

    private float rotation = 0;

    private void Start() {
        rotation = startingRotation;
    }

    private void Update() {
        rotation -= speed * Time.deltaTime; ;
        material.SetFloat("_Rotation", rotation);
    }



}
