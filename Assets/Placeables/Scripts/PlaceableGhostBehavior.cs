using System.Linq;
using UnityEngine;

public class PlaceableGhostBehavior : PlaceableBehavior, IPoolableItem {

    [SerializeField] private float buildTime;
    [SerializeField] private Material materialBlue;
    [SerializeField] private Material materialRed;
    [SerializeField] private MeshRenderer[] meshRenderers;
    private bool readyToBuild = false;
    private float currentBuildingTime = 0;

    public void UpdatePreview() {
        UpdateMaterial();
    }

    public void Reset() {
        readyToBuild = false;
        currentBuildingTime = 0;
    }

    public void SetReadyToBuild(bool readyToBuild) {
        this.readyToBuild = readyToBuild;
    }

    public bool IsReadyToBuild() {
        return readyToBuild;
    }

    public bool AddProgressToBuild(float time) {
        if (readyToBuild) {
            currentBuildingTime += time;
            if (IsBuildCompleted()) {
                FinishBuild();
                return true;
            }
        } else {
            Debug.LogError("You can't call AddProgressToBuild to a GhostPlaceable not yet ready to build.");
        }
        return false;
    }

    public bool IsBuildCompleted() {
        return currentBuildingTime >= buildTime;
    }

    private void FinishBuild() {
        PlaceableBehavior placeable = PoolManager.Instance.GetPoolByType(type).GetNext().GetComponent<PlaceableBehavior>();
        placeable.transform.position = this.transform.position;
        placeable.transform.forward = this.transform.forward;
        placeable.transform.SetParent(this.transform.parent);
        this.gameObject.SetActive(false);
        GridSystemManager.Instance.TakeSpace(placeable);
    }

    private void UpdateMaterial() {
        bool isEnoughtSpace = GridSystemManager.Instance.IsGridEmpty(this);
        Material material = isEnoughtSpace ? materialBlue : materialRed;
        SetAllMaterialsTo(material);
    }

    private void SetAllMaterialsTo(Material material) {
        foreach (MeshRenderer mesh in meshRenderers) {
            int meshMaterialCount = mesh.materials.Length;
            Material[] newMaterialArray = Enumerable.Repeat(material, meshMaterialCount).ToArray();
            mesh.materials = newMaterialArray;
        }
    }

}
