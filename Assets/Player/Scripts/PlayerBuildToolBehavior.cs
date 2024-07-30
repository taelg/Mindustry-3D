using UnityEngine;

public class PlayerBuildToolBehavior : MonoBehaviour {

    [SerializeField] private LightBeamBehavior lightBeam;

    private PlaceableGhostBehavior currentGhost;
    private PlaceableGhostBehavior currentDestroy;


    private void Update() {
        UpdateCurrentGhost();
        BuildNextGhost();

        DestroyPlaceables();
    }

    private void UpdateCurrentGhost() {
        if (!currentGhost && PlaceModeManager.Instance.placedGhosts.Count > 0) {
            currentGhost = PlaceModeManager.Instance.placedGhosts.Dequeue();
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
        while (PlaceModeManager.Instance.placeablesToDestroy.Count > 0) {
            PlaceableBehavior placeable = PlaceModeManager.Instance.placeablesToDestroy.Dequeue();
            GridSystemManager.Instance.LeaveSpace(placeable);
            placeable.gameObject.SetActive(false);
        }

    }


}
