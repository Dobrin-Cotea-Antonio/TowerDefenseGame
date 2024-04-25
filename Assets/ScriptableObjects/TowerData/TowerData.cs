using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[System.Serializable]
public class UpgradeData {
    public int upgradeCost;
    public float upgradeValue;
   
}

[CreateAssetMenu(fileName = "TowerData", menuName = "TowerData")]
public class TowerData : ScriptableObject{

    public static int maxUpgradeLevel = 2;

    [SerializeField] string _towerName;
    [SerializeField] float _projectileSpeed;
    [SerializeField] float _attackCooldown;
    [SerializeField] int _attackDamage;
    [SerializeField] float _attackRange;
    [SerializeField] int _cost;
    [SerializeField] float _sellValuePercentage;
    public List<TargetingType> allowedTargetingTypes;
    [SerializeField] bool _rotateWeapon;
    [SerializeField] GameObject _towerPrefab;
    [SerializeField] GameObject _hoverPrefab;
    [SerializeField] string _description;
    [SerializeField] float _AOESize;
    [SerializeField] Sprite _sprite;

    public string towerName { get { return _towerName; } }
    public float projectileSpeed { get { return _projectileSpeed; } }
    public float attackCooldown { get { return _attackCooldown; } private set { _attackCooldown = value; } }
    public int attackDamage { get { return _attackDamage; } private set { _attackDamage = value; } }
    public float attackRange { get { return _attackRange; } private set { _attackRange = value; } }
    public int cost { get { return _cost; } }
    public float sellValuePercentage { get { return _sellValuePercentage; } }
    public bool rotateWeapon { get { return _rotateWeapon; } }

    public GameObject towerPrefab { get { return _towerPrefab; } }

    public GameObject hoverPrefab { get { return _hoverPrefab; } }

    public string description { get { return _description; } }

    public float AOESize { get { return _AOESize; } }

    public Sprite sprite { get { return _sprite; } }

}
