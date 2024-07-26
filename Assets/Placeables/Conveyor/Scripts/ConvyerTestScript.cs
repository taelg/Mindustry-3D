using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConvyerTestScript : MonoBehaviour {

    [SerializeField] private float speed = 100;
    [SerializeField] private float startingOffset = 0.09f;
    [SerializeField] private Material material;

    private void Start() {
        material.mainTextureOffset = new Vector2(startingOffset, 0);
    }

    private void Update() {
        material.mainTextureOffset -= new Vector2(1, 0) * (speed / 100f) * Time.deltaTime;
    }
}
