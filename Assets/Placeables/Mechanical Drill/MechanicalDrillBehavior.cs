using System.Collections;
using UnityEngine;

public class MechanicalDrillBehavior : FlowStructureBehavior, IBuildable {


    [Header("Configurable")]
    [Tooltip("Maximum tier of blocks this drill can mine.")]
    [SerializeField] private int tier = 1;
    [Tooltip("Base time to drill one ore, in seconds.")]
    [SerializeField] private float drillTime = 11f;
    [SerializeField] private int maxOreCapacity = 10;
    [SerializeField][Range(0.1f, 1f)] private float drillSpeed;
    [SerializeField][Range(0.1f, 4f)] private float secondsToMaxSpeed;

    [Space]
    [Header("Internal")]
    [SerializeField] private MultiTileOreCheckerBehavior oreChecker;
    [SerializeField] private Transform drill;

    private float acceleration;
    private bool starting = false;
    private bool producing = false;
    private float oreCount;

    private float currentSpeed = 0;


    public void OnBuild() {
        //TODO: Fix model. The model show some imperfection on rotate.
        this.transform.forward = Vector3.forward;
    }

    private void Update() {
        SpinDrill();
        RestartDrill();
    }

    private void StartDrill() {
        acceleration = drillSpeed / (secondsToMaxSpeed / 0.02f);
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

    private void SpinDrill() {
        drill.Rotate(0f, -currentSpeed, 0f, Space.Self);
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
        bool drillTierIsEnough = oreChecker.GetOreTier() <= tier;

        while (!IsFull() && drillTierIsEnough) {
            float timeToDrillOne = drillTime / oreChecker.GetEffectiveTileCount();
            yield return new WaitForSeconds(timeToDrillOne);
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
