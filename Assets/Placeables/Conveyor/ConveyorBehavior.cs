using System.Collections.Generic;
using UnityEngine;

public class ConveyorBehavior : FlowStructureBehavior {

    [Header("Internal")]
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private MeshRenderer belt;
    [SerializeField] private Material oneInput;
    [SerializeField] private Material oneInputCurve;
    [SerializeField] private Material twoInputsSides;
    [SerializeField] private Material twoInputsBackSide;
    [SerializeField] private Material treeInputs;

    public Vector3 outputDir;

    public void OnPlace(Vector3 outputDir) {
        this.outputDir = outputDir;
        UpdateConveyorInputs();
    }

    private void OnEnable() {
        outputDir = this.transform.forward;
        outputs.Clear();
        outputs.Add(outputDir);
    }

    public Vector3 GetOutputDirection() {
        return outputDir;
    }

    private void UpdateConveyorInputs() {
        bool incomeForward = (bool)GetNeighbourOutputingToDir(this.transform.forward);
        bool incomeBack = (bool)GetNeighbourOutputingToDir(-this.transform.forward);
        bool incomeRight = (bool)GetNeighbourOutputingToDir(this.transform.right);
        bool incomeLeft = (bool)GetNeighbourOutputingToDir(-this.transform.right);

        int inputCount = 0;
        inputCount += incomeForward ? 1 : 0;
        inputCount += incomeRight ? 1 : 0;
        inputCount += incomeLeft ? 1 : 0;
        inputCount += incomeBack ? 1 : 0;
        if (inputCount == 0)
            return;

        if (inputCount == 1) {
            belt.material = oneInput;
            Debug.Log("oneInput");
            if (incomeRight) {
                belt.material = oneInputCurve;
                Debug.Log("oneInputCurve RIGHT");
            } else if (incomeLeft) {
                belt.material = oneInputCurve;
                this.transform.localScale = new Vector3(-1, 1, 1);
                Debug.Log("oneInputCurve LEFT");
            }
        } else if (inputCount == 2) {
            bool typeSides = incomeLeft && incomeRight;
            bool typeBackSide = incomeBack && (incomeLeft || incomeRight);
            belt.material = typeSides ? twoInputsSides : twoInputsBackSide;
            if (twoInputsBackSide && incomeRight)
                this.transform.localScale = new Vector3(-1, 1, 1);
            Debug.Log("twoInputs");
        } else if (inputCount == 3) {
            belt.material = treeInputs;
            Debug.Log("treeInputs");
        }





    }

    private void FaceDirection(UnityEngine.Vector3 direction) {
        this.transform.right = direction;
    }

    private FlowStructureBehavior GetNeighbourOutputingToDir(Vector3 direction) {
        if (direction == outputDir)
            return null;

        Ray ray = new Ray(rayOrigin.transform.position, direction);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1f);
        Debug.DrawRay(rayOrigin.transform.position, direction, Color.green, 10);
        bool hitSomething = !hit.transform;
        if (hitSomething)
            return null;

        FlowStructureBehavior flowStruct = hit.transform.GetComponent<FlowStructureBehavior>();
        bool isFlowStructure = (bool)flowStruct;
        if (!isFlowStructure)
            return null;

        List<Vector3> outputs = flowStruct.GetOuputList();
        bool outputToSelectedDir = false;
        outputs.ForEach(output => {
            if (output == (-direction))
                outputToSelectedDir = true;
        });

        if (!outputToSelectedDir)
            return null;

        return hit.transform.GetComponent<FlowStructureBehavior>();
    }

    void Update() {

    }

    public bool IsInputSide(Vector3 direction) {
        return direction != outputDir;
    }

    public bool IsOutputSide(Vector3 direction) {
        return outputDir == direction;
    }

}
