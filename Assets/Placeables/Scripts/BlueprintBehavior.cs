using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BlueprintBehavior : BuildingBehavior, IPoolableItem {

    [SerializeField] private float buildTime;
    [SerializeField] private bool rotable;
    [SerializeField] protected Material materialBlue;
    [SerializeField] protected Material materialRed;
    [SerializeField] private MeshRenderer[] meshRenderers;
    protected bool canPlaceHere = false;
    private bool readyToBuild = false;
    private float currentBuildingTime = 0;

    public bool TryPlaceHere() {
        if (!canPlaceHere)
            return false;

        GridSystemManager.Instance.TakeSpace(this);
        return true;
    }

    public virtual void UpdatePreview() {
        bool isEnoughtSpace = GridSystemManager.Instance.IsGridEmpty(this);
        canPlaceHere = isEnoughtSpace;
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

    public bool IsRotable() {
        return rotable;
    }

    public bool AddProgressToBuild(float time) {
        if (readyToBuild) {
            currentBuildingTime += time;
            if (IsBuildCompleted()) {
                FinishBuild();
                return true;
            }
        } else {
            Debug.LogError("You can't call AddProgressToBuild to a Blueprint not yet ready to build.");
        }
        return false;
    }

    public bool IsBuildCompleted() {
        return currentBuildingTime >= buildTime;
    }

    private void FinishBuild() {
        BuildingBehavior building = BuildingPoolManager.Instance.GetBuildingPool(type).GetNext().GetComponent<BuildingBehavior>();
        building.transform.position = this.transform.position;
        building.transform.forward = this.transform.forward;
        building.transform.SetParent(this.transform.parent);
        this.gameObject.SetActive(false);
        GridSystemManager.Instance.TakeSpace(building);
        building.GetComponent<BuildingBehavior>().OnBuild();
    }

    protected void UpdateMaterial() {
        Material material = canPlaceHere ? materialBlue : materialRed;
        SetAllMaterialsTo(material);
    }

    protected void SetAllMaterialsTo(Material material) {
        foreach (MeshRenderer mesh in meshRenderers) {
            int meshMaterialCount = mesh.materials.Length;
            Material[] newMaterialArray = Enumerable.Repeat(material, meshMaterialCount).ToArray();
            mesh.materials = newMaterialArray;
        }
    }

    public override void OnBuild() {
        Debug.LogError("Code Should never reach here. After built it's no longer a Blueprint but the actual Building Object.");
    }

}
