using UnityEngine;

public class PlayerBuildToolBehavior : MonoBehaviour {

    [SerializeField] private LightBeamBehavior lightBeam;

    private BlueprintBehavior currentGhost;
    private BlueprintBehavior currentDestroy;


    private void Update() {
        UpdateCurrentGhost();
        BuildNextGhost();

        DestroyPlaceables();
    }

    private void UpdateCurrentGhost() {
        if (!currentGhost && PlacementManager.Instance.blueprintsPlaced.Count > 0) {
            currentGhost = PlacementManager.Instance.blueprintsPlaced.Dequeue();
        }
    }

    private void BuildNextGhost() {
        if (currentGhost) {
            lightBeam.SetLightBeamTarget(currentGhost);
            bool finished = currentGhost.AddProgressToBuild(Time.deltaTime);
            if (finished) {
                currentGhost = null;
            }
        }
    }

    private void DestroyPlaceables() {
        while (PlacementManager.Instance.itemsToDestroy.Count > 0) {
            PlaceableBehavior placeable = PlacementManager.Instance.itemsToDestroy.Dequeue();
            GridSystemManager.Instance.LeaveSpace(placeable);
            placeable.gameObject.SetActive(false);
        }

    }


}
