using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AYellowpaper.SerializedCollections;


/// <summary>
/// Class <c>MaterialInfo</c> is used to store data about what material needs to be changed on a tower once its upgraded. It also contains the new material.
/// </summary>
[System.Serializable]
public class MaterialInfo {
    public int index;
    public Material replaceMaterial;
    public MeshRenderer renderer;
}

/// <summary>
/// Class <c>Upgrade</c> is used to upgrade a tower and change its visual aspect.
/// </summary>
[RequireComponent(typeof(TowerController))]
public abstract class Upgrade : MonoBehaviour{

    [Header("Data")]
    [SerializeField] protected int _upgradeCost;
    [SerializeField] protected MaterialInfo[] info;

    public int upgradeCost { get { return _upgradeCost; } }
    

    public bool isActive { get; protected set; }
    protected TowerController targetTower;

    protected virtual void Start(){
        isActive = false;
        targetTower = GetComponent<TowerController>();
    }

    public abstract void ApplyUpgrade();
    public abstract void FormatDescription(TextMeshProUGUI pText);
    public abstract void FormatUpgradeText(TextMeshProUGUI pText);

    protected void ApplyMaterials() {

        foreach (MaterialInfo i in info) {
            Material[] materials = i.renderer.materials;
            materials[i.index] = i.replaceMaterial;
            i.renderer.materials = materials;

        }


    }



}



