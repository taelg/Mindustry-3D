using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MechanicalDrillBehavior : MonoBehaviour {


    [Header("Configurable")]
    [Tooltip("Maximum tier of blocks this drill can mine.")]
    [SerializeField] private int tier = 1;
    [Tooltip("Base time to drill one ore, in seconds.")]
    [SerializeField] private float drillTime = 11f;
    [SerializeField] private int maxOreCapacity = 10;
    [SerializeField][Range(0.5f, 5f)] private float drillSpeed;
    [SerializeField][Range(5f, 50f)] private float acceleration;

    [Header("Read Only")]
    [SerializeField] private float secondsToStartProduction = 0;

    [Space]
    [Header("Internal")]
    [SerializeField] private MultiTileOreCheckerBehavior oreChecker;
    [SerializeField] private Transform drill;

    private bool starting = false;
    private bool producing = false;
    private float oreCount;

    private float currentSpeed = 0;

    private void StartDrill() {
        Debug.Log("StartDrill");
        starting = true;
        StopAllCoroutines();
        StartCoroutine(SpeedUpDrill());
        StartCoroutine(Production());

    }

    private void StopDrill() {
        StopAllCoroutines();
        StartCoroutine(SpeedDownDrill());
        StopCoroutine(Production());
    }

    private void Update() {
        UpdateReadOnlyInformation();
        SpinDrill();
        RestartDrill();
    }

    private void UpdateReadOnlyInformation() {
        secondsToStartProduction = drillSpeed / (acceleration / 1000) * 0.02f;
    }

    private void SpinDrill() {
        drill.Rotate(0f, 0f, -currentSpeed, Space.Self);
    }

    private void RestartDrill() {
        if (!producing && !IsFull() && !starting) {
            oreChecker.Recalculate();
            StartDrill();
        }
    }

    private IEnumerator SpeedUpDrill() {
        while (currentSpeed < drillSpeed) {
            currentSpeed += (acceleration / 1000);
            yield return new WaitForSeconds(0.02f);
        }
        currentSpeed = drillSpeed;
        starting = false;
        producing = true;
    }

    private IEnumerator SpeedDownDrill() {
        while (currentSpeed > 0) {
            currentSpeed -= (acceleration / 1000);
            yield return new WaitForSeconds(0.02f);
        }
        currentSpeed = 0;
    }

    private IEnumerator Production() {
        yield return new WaitUntil(() => IsAtMaxSpeed());
        oreChecker.Recalculate();
        Debug.Log("Recalculated: " + oreChecker.GetEffectiveTileCount());


        while (!IsFull()) {
            float timeToDrillOne = drillTime / oreChecker.GetEffectiveTileCount();
            Debug.Log("timeToDrillOne: " + timeToDrillOne);
            yield return new WaitForSeconds(timeToDrillOne);
            Debug.Log("drilled!");
            oreCount++;
        }

        producing = false;
        StopDrill();
    }

    private bool IsFull() {
        return oreCount >= maxOreCapacity;
    }

    private bool IsAtMaxSpeed() {
        return currentSpeed >= drillSpeed;
    }


}
