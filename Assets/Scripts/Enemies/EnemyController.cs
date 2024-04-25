using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Class <c>EnemyController</c> is used to control the enemies,make them take damage and apply status effects. Contains an IDamagable interface and an IPathfinder.
/// </summary>
[RequireComponent(typeof(IPathFinder)), RequireComponent(typeof(NavMeshPathfinder))]
public class EnemyController : MonoBehaviour, IDamagable{

    public Action<int> OnEnemyDeath;
    public Action<int> OnEnemySpawn;
    public Action<int,int> OnDamageTaken;
    public Action<StatusEffect> OnStatusCreate;
    public Action<StatusEffect> OnStatusRemove;

    [Header("Data")]
    [SerializeField] EnemyData _data;
    [SerializeField] int _damage;
    [SerializeField] GameObject deathMoneyIndicator;
    [SerializeField] GameObject explosionEffectPrefab;
    public EnemyData data { get { return _data; } }
    public int damage { get { return _damage; } }

    [Header("Hp")]
    int maxHp;
    int currentHp;
    public float speed { get; set; }

    List<StatusEffect> statusEffects=new List<StatusEffect>();

    IPathFinder pathFinder;
    public Waypoint targetWaypoint;

    private void Start(){
        pathFinder = GetComponent<IPathFinder>();
        maxHp = _data.maxEnemyHP;
        currentHp = maxHp;
        speed = _data.speed;
        OnEnemyDeath += SpawnMoneyDropText;
        OnEnemyDeath += SpawnExplosion;
    }


    private void Update(){
        if (!pathFinder.MoveTowardsTarget(targetWaypoint.transform.position, speed)) {
            targetWaypoint = targetWaypoint.ReturnNextWaypoint();
        }
        
    }

    public void TakeDamage(int pDamage) {

        if (CheatMenu.instance != null ) {
            if (CheatMenu.instance.instantEnemyKill)
                pDamage = int.MaxValue;
        }


        currentHp -= pDamage;
        OnDamageTaken?.Invoke(currentHp,maxHp);
        if (currentHp <= 0) {
            OnEnemyDeath?.Invoke(_data.moneyReward);
            Destroy(gameObject);
        }
    }

    public void ApplyStatusEffect(GameObject pStatusEffectPrefab) {

        StatusEffect status = Instantiate(pStatusEffectPrefab, transform).GetComponent<StatusEffect>();

        if (status == null)
            return;

        status.SetTarget(this);
        status.OnDestroy += RemoveEffectFromList;

        bool wasStatusFound = false;
        StatusEffect identicalStatus = null;

        foreach (StatusEffect effect in statusEffects) {
            if (effect.GetType() == status.GetType()) {
                wasStatusFound = true;
                identicalStatus = effect;
                break;
            }

        }

        if (!wasStatusFound){
            statusEffects.Add(status);
            status.Apply();
            OnStatusCreate?.Invoke(status);

        } else {
            identicalStatus.StackEffect();
            Destroy(status.gameObject);
        }
    
    }

    void RemoveEffectFromList(StatusEffect pStatus) {
        pStatus.OnDestroy -= RemoveEffectFromList;
        OnStatusRemove?.Invoke(pStatus);
        statusEffects.Remove(pStatus);
    }

    public void SetTargetWaypoint(Waypoint pWaypoint) {
        targetWaypoint = pWaypoint;
    }

    void SpawnMoneyDropText(int pMoney) {

        MoneyDropText text = Instantiate(deathMoneyIndicator, transform.position, Quaternion.identity).GetComponent<MoneyDropText>();
        text.SetText(string.Format("+{0}",pMoney));
        
    }

    void SpawnExplosion(int pValue) {
        GameObject g = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(g, 1.5f);
    }

}