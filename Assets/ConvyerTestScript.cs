using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConvyerTestScript : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private Material material;
    [SerializeField] private bool moveInX;
    [SerializeField] private bool moveInY;


    void Update() {
        float x = moveInX ? 1 : 0;
        float y = moveInY ? 1 : 0;
        material.mainTextureOffset -= new Vector2(x, y) * speed * Time.deltaTime;
    }
}
