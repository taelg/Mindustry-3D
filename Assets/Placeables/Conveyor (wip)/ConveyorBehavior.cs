using UnityEngine;

public class Conveyor : FlowBuildingBehavior, IPoolableItem {

    [Header("Internal")]
    [SerializeField] private MeshRenderer belt;
    [SerializeField] private Material oneInput;
    [SerializeField] private Material oneInputCurve;
    [SerializeField] private Material twoInputsSides;
    [SerializeField] private Material twoInputsBackSide;
    [SerializeField] private Material treeInputs;

    public Vector3 outputDir;

    public void Reset() {
    }

    public override void OnBuild() {
        outputDir = this.transform.forward;
        SetSingleOutputDir(outputDir);
        UpdateConveyorInputs();
    }

    private void UpdateConveyorInputs() {
        bool incomeBack = (bool)GetNeighbourOutputingToDir(-this.transform.forward);
        bool incomeRight = (bool)GetNeighbourOutputingToDir(this.transform.right);
        bool incomeLeft = (bool)GetNeighbourOutputingToDir(-this.transform.right);

        int inputCount = 0;
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

    public bool IsInputSide(Vector3 direction) {
        return direction != outputDir;
    }

    public bool IsOutputSide(Vector3 direction) {
        return outputDir == direction;
    }

}
