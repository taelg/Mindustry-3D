using System.Collections.Generic;
using UnityEngine;

public class FlowStructureBehavior : MonoBehaviour {

    [SerializeField] protected List<Vector3> inputs;
    [SerializeField] protected List<Vector3> outputs;

    public int GetInputCount() {
        return inputs.Count;
    }

    public int GetOutputCount() {
        return outputs.Count;
    }

    public List<Vector3> GetOuputList() {
        return outputs;
    }

}
