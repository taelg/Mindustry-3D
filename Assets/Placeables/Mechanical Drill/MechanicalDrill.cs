using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalDrill : FlowBuildingBehavior, IPoolableItem {


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
    [SerializeField] private Transform drill;

    private float currentAcceleration = 0;
    private float currentSpeed = 0;
    private float currentOreCount = 0;
    private OreType foundOreType = OreType.NONE;
    private int foundOreTier = 1;
    private int effectiveOreTileCount = 0;


    public void Reset() {
        Debug.Log("Reset");
        this.transform.forward = Vector3.forward;
        foundOreType = OreType.NONE;
        currentSpeed = 0;
    }

    public override void OnBuild() {
        Debug.Log("On Build");
        CalculateOreInformation();
        Debug.Log("foundOreType.........: " + foundOreType);
        Debug.Log("effectiveOreTileCount: " + effectiveOreTileCount);
        StartDrill();
    }
    private void CalculateOreInformation() { //TODO: this method is reapeted on MechanicalDrillBlueprint
        List<Tile> ownTiles = GetOwnTiles();
        effectiveOreTileCount = 0;

        foreach (Tile tile in ownTiles) {
            OreType foundOreType = tile.oreType;
            if (foundOreType == OreType.NONE)
                continue;

            bool noOreFoundYet = this.foundOreType == OreType.NONE;
            if (noOreFoundYet) {
                this.foundOreType = foundOreType;
                this.foundOreTier = 1; //TODO: Implement Ore Tiers.
            }
            effectiveOreTileCount++;
        }
    }

    private void Update() {
        SpinDrill();
    }

    private void StartDrill() {
        Debug.Log("foundOreType: " + foundOreType);
        if (foundOreType == OreType.NONE)
            return;

        Debug.Log("effectiveness: " + effectiveOreTileCount);
        currentAcceleration = drillSpeed / (secondsToMaxSpeed / 0.02f);
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

    private IEnumerator SpeedUpDrill() {
        while (currentSpeed < drillSpeed) {
            currentSpeed += (currentAcceleration / 1000);
            yield return new WaitForSeconds(0.02f);
        }
        currentSpeed = drillSpeed;
    }

    private IEnumerator SpeedDownDrill() {
        while (currentSpeed > 0) {
            currentSpeed -= currentAcceleration / 1000;
            yield return new WaitForSeconds(0.02f);
        }
        currentSpeed = 0;
    }

    private IEnumerator Production() {
        yield return new WaitUntil(() => IsAtMaxSpeed());
        bool drillTierIsEnough = foundOreTier <= tier;

        while (!IsFull() && drillTierIsEnough) {
            float timeToDrillOne = drillTime / effectiveOreTileCount;
            yield return new WaitForSeconds(timeToDrillOne);
            currentOreCount++;
        }

        StopDrill();
    }

    private bool IsFull() {
        return currentOreCount >= maxOreCapacity;
    }

    private bool IsAtMaxSpeed() {
        return currentSpeed >= drillSpeed;
    }

}
