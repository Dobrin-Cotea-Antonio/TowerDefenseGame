using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AYellowpaper.SerializedCollections;

public enum TargetingType {
    First,
    Last,
    Closest,
    Furthest,
    Tankiest,
    Fastest,
    Manual
}


/// <summary>
/// Class <c>TowerController</c> is used to control the towers. It provides different targeting types and rotates the tower towards the targeted enemy.
/// </summary>
[RequireComponent(typeof(IAttacker))]
public class TowerController : MonoBehaviour {

    [Header("Data")]
    [SerializeField] TowerData _data;

    public int damage { get; set; }
    public float range { get; set; }
    public float attackCooldown { get; set; }

    public float projectileSpeed { get; set; }

    public TowerData data { get { return _data; } }

    [Header("Rotation Transforms")]
    [SerializeField] Transform towerBaseTransform;
    [SerializeField] Transform weaponTransform;

    //targeting
    public TargetingType targetingType { get; private set; }
    IAttacker attacker;
    int targetingTypesCount;

    [Header("Target Interaction")]
    [SerializeField] Transform rangeHolder;
    [SerializeField] Transform rangeIndicator; 
    Transform currentTarget;

    float unit;
    float lastShotTime = -10000000;
    TowerTargets targets;

    [Header("Upgrades")]
    [SerializeField] SerializedDictionary<int, List<Upgrade>> upgradeObjects;
    public Dictionary<int, int> upgradeLevel { get; private set; }

    void Start() {
        unit = 1f / towerBaseTransform.localScale.x;
        rangeHolder.localScale = new Vector3(unit * _data.attackRange, 1, unit * _data.attackRange);
        targets = rangeHolder.GetComponent<TowerTargets>();
        attacker = GetComponent<IAttacker>();

        string[] names = Enum.GetNames(typeof(TargetingType));
        targetingTypesCount = names.Length;
        targetingType = data.allowedTargetingTypes[0];

        damage = data.attackDamage;
        range = data.attackRange;
        attackCooldown = data.attackCooldown;
        projectileSpeed = data.projectileSpeed;

        upgradeLevel = new Dictionary<int, int>();
        for (int i = 0; i < 3; i++) 
            upgradeLevel[i] = 0;
        
    }

    void Update(){
        rangeHolder.localScale = new Vector3(unit * range, 1, unit * range);

        ChooseTarget();
        RotateTowardsTarget();

        if (currentTarget != null && Time.time - lastShotTime > attackCooldown) {
            attacker.Attack(currentTarget, projectileSpeed, damage, range);
            lastShotTime = Time.time;
        }

    }

    void RotateTowardsTarget() {

        if (currentTarget == null)
            return;

        //rotates the base
        Vector3 targetPos = new Vector3(currentTarget.position.x, towerBaseTransform.position.y, currentTarget.position.z);
        Vector3 direction = (targetPos - towerBaseTransform.position).normalized;
        towerBaseTransform.forward = direction;

        //rotates the weapon itself
        if (!_data.rotateWeapon)
            return;

        Vector3 weaponDirection = (currentTarget.position - weaponTransform.position).normalized;
        weaponTransform.forward = weaponDirection;
    }

    void ChooseTarget() {

        if (currentTarget != null) {
            if (!targets.enemiesInRange.Contains(currentTarget))
                currentTarget = null;
            return;
        }

        if (targets.enemiesInRange.Count == 0)
            return;

        switch (targetingType) {
            case TargetingType.Closest:
                currentTarget = targets.ReturnClosestEnemy();
                break;
            case TargetingType.Furthest:
                currentTarget = targets.ReturnFurthestEnemy();
                break;
            case TargetingType.Tankiest:
                currentTarget = targets.ReturnTankiestEnemy();
                break;
            case TargetingType.Fastest:
                currentTarget = targets.ReturnFastestEnemy();
                break;
            case TargetingType.First:
                currentTarget=targets.enemiesInRange[0];
                break;
            case TargetingType.Last:
                currentTarget = targets.enemiesInRange[targets.enemiesInRange.Count-1];
                break;
            default:
                Debug.Log("Not Implemented");
                break;
        }

    }

    public Transform RequestTarget() {
        ChooseTarget();
        return currentTarget;
    }

    public int ReturnTowerValue() {

        int value = (int)(data.cost * (data.sellValuePercentage / 100));

        foreach (KeyValuePair<int, int> upgradeRow in upgradeLevel) {
            for (int i = 0; i < upgradeRow.Value; i++) {
                value += (int)(upgradeObjects[upgradeRow.Key][i].upgradeCost*(data.sellValuePercentage/100));
            }
        }

        return value;

    }

    public void CycleTargetingType(bool pForward) {

        int value = 0;
        if (pForward)
            value = 1;
        else
            value = -1;

        targetingType = (TargetingType)(((int)(targetingType + value + targetingTypesCount)) % targetingTypesCount);
        while (!data.allowedTargetingTypes.Contains(targetingType)) {
            targetingType = (TargetingType)(((int)(targetingType + value + targetingTypesCount)) % targetingTypesCount);
        }
    }

    public void IncreaseUpgradeLevel(int pIndex) {
        if (upgradeLevel[pIndex] >= TowerData.maxUpgradeLevel)
            return;
        upgradeObjects[pIndex][upgradeLevel[pIndex]].ApplyUpgrade();
        upgradeLevel[pIndex]++;
    }
    //used for getting the next upgrade
    public Upgrade GetNextUpgrade(int pIndex) {
        if (upgradeLevel[pIndex] >= TowerData.maxUpgradeLevel)
            return null;
        return upgradeObjects[pIndex][upgradeLevel[pIndex]];
    }

    public int GetUpgradeLevel(int pIndex) {
        return upgradeLevel[pIndex];
    }

    //returns an upgrade object that can be used to format the ui text in all possible cases
    public Upgrade GetCurrentUpgrade(int pIndex){
        if (upgradeLevel[pIndex] >= TowerData.maxUpgradeLevel)
            return upgradeObjects[pIndex][TowerData.maxUpgradeLevel - 1];
        else
            return upgradeObjects[pIndex][upgradeLevel[pIndex]];
        
    }

    public void ChangeRangeIndicatorState(bool pState) {
        rangeIndicator.gameObject.SetActive(pState);
    }

}
