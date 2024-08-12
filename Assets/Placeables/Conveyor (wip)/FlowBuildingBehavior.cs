using System.Collections.Generic;
using UnityEngine;

public abstract class FlowBuildingBehavior : BuildingBehavior {

    [SerializeField] protected int itemCapacity = 3;
    [SerializeField] protected List<Vector3> inputDirections;
    [SerializeField] protected List<Vector3> outputDirections;
    protected List<OreType> itemsInside;

    public void SetSingleOutputDir(Vector3 dir) {
        outputDirections.Clear();
        outputDirections.Add(dir);
    }

    public List<Vector3> GetInputDirections() {
        return inputDirections;
    }

    public List<Vector3> GetOutputDirections() {
        return outputDirections;
    }

    //TODO: this method was written before grid system, now we have to update it to ask neighbour tiles from grid system.
    protected FlowBuildingBehavior GetNeighbourOutputingToDir(Vector3 direction) {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1f);
        Debug.DrawRay(transform.position, direction, Color.green, 10);
        bool hitSomething = !hit.transform;
        if (hitSomething)
            return null;

        FlowBuildingBehavior flowStruct = hit.transform.GetComponent<FlowBuildingBehavior>();
        bool isFlowStructure = (bool)flowStruct;
        if (!isFlowStructure)
            return null;

        List<Vector3> outputs = flowStruct.GetOutputDirections();
        bool outputToSelectedDir = false;
        outputs.ForEach(output => {
            if (output == (-direction))
                outputToSelectedDir = true;
        });

        if (!outputToSelectedDir)
            return null;

        return hit.transform.GetComponent<FlowBuildingBehavior>();
    }

}
