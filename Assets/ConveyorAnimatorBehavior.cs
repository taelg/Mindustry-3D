using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorAnimatorBehavior : MonoBehaviour {
    [SerializeField] Material material;
    [SerializeField] float[] xOffsetSteps = new float[4];

    private float currentXOffset;
    private int index = 0;

    private void Start() {
        currentXOffset = 0;
        StartCoroutine(Animate());

    }

    private IEnumerator Animate() {
        while (this.gameObject.activeSelf) {
            yield return new WaitForSeconds(0.2f);
            index++;
            index = xOffsetSteps.Length == index ? 0 : index;
            currentXOffset = xOffsetSteps[index];
            material.SetTextureOffset("_MainTex", new Vector2(currentXOffset, 0));
        }
    }

}
