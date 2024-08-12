using UnityEngine;

public class PlayerBuildToolBehavior : MonoBehaviour {

    [SerializeField] private LightBeamBehavior lightBeam;

    private BlueprintBehavior currentBlueprint;


    private void Update() {
        UpdateCurrentBlueprint();
        BuildNextBlueprint();

        DestroyBuildings();
    }

    private void UpdateCurrentBlueprint() {
        if (!currentBlueprint && PlacementManager.Instance.blueprintsPlaced.Count > 0) {
            currentBlueprint = PlacementManager.Instance.blueprintsPlaced.Dequeue();
        }
    }

    private void BuildNextBlueprint() {
        if (currentBlueprint) {
            lightBeam.SetLightBeamTarget(currentBlueprint);
            bool finished = currentBlueprint.AddProgressToBuild(Time.deltaTime);
            if (finished) {
                currentBlueprint = null;
            }
        }
    }

    private void DestroyBuildings() {
        while (PlacementManager.Instance.itemsToDestroy.Count > 0) {
            BuildingBehavior building = PlacementManager.Instance.itemsToDestroy.Dequeue();
            GridSystemManager.Instance.LeaveSpace(building);
            building.gameObject.SetActive(false);
        }

    }


}
