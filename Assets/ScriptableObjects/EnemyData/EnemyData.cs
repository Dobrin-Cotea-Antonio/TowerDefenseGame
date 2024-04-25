using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemies", menuName = "Enemy")]
public class EnemyData : ScriptableObject {

    [SerializeField] int _maxEnemyHP;
    [SerializeField] float _speed;
    [SerializeField] int _moneyReward;
    //[SerializeField] float _damage;
    
    public int maxEnemyHP { get { return _maxEnemyHP; } private set { _maxEnemyHP = value; } }
    public float speed { get { return _speed; } private set { _speed = value; } }
    public int moneyReward { get { return _moneyReward; } private set { _moneyReward = value; } }
    //public float damage { get { return _damage; } private set { _damage = value; } }


}
