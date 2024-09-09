using System.Collections;
using UnityEngine;

public class SunBehavior : MonoBehaviour {

    [SerializeField] float dayDurationInMinutes = 60;

    private void Start() {
        StartCoroutine(RotateSun());
    }

    private IEnumerator RotateSun() {
        float dayDurationInSeconds = dayDurationInMinutes * 60f;
        float rotationPerSecond = 360f / dayDurationInSeconds;

        while (true) {
            yield return new WaitForSeconds(0.1f);
            this.transform.Rotate(new Vector3(rotationPerSecond / 10f, 0, 0));
        }

    }

}
