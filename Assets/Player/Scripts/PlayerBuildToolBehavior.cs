using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBuildToolBehavior : MonoBehaviour {

    [SerializeField] private float radiusRange;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private LightBeamBehavior lightBeam;

    private List<PlaceableGhostBehavior> ghostsInRange = new List<PlaceableGhostBehavior>();

    private void Awake() {
        sphereCollider.radius = radiusRange;
    }

    private void Update() {
        UpdateGhostsInRange();
        BuildNearestGhost();
    }

    private void UpdateGhostsInRange() {
        ghostsInRange.Clear();
        Collider[] collidersInside = Physics.OverlapSphere(transform.position, radiusRange);

        foreach (Collider collider in collidersInside) {
            PlaceableGhostBehavior ghost = collider.transform.GetComponent<PlaceableGhostBehavior>();
            if (ghost) {
                ghostsInRange.Add(ghost);
            }
        }
    }

    private void BuildNearestGhost() {
        PlaceableGhostBehavior ghost = GetNearestGhost(GetReadyToBuildGhosts());
        if (ghost) {
            lightBeam.SetLightBeamTarget(ghost);
            ghost.AddProgressToBuild(Time.deltaTime);
            ghostsInRange.Remove(ghost);

        }
    }


    private List<PlaceableGhostBehavior> GetReadyToBuildGhosts() {
        return ghostsInRange.Where(ghost => ghost.IsReadyToBuild()).ToList();
    }

    private PlaceableGhostBehavior GetNearestGhost(List<PlaceableGhostBehavior> readyToBuild) {
        PlaceableGhostBehavior nearestGhost = null;
        float nearestDistance = float.MaxValue;

        foreach (PlaceableGhostBehavior ghost in readyToBuild) {
            float distance = (ghost.transform.position - transform.position).magnitude;
            if (distance < nearestDistance) {
                nearestGhost = ghost;
                nearestDistance = distance;
            }
        }

        return nearestGhost;
    }



}
