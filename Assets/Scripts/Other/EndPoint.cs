using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class <c>EndPoint</c> is used to detect when an enemy has reached the final destination. 
/// </summary>
public class EndPoint : MonoBehaviour{

    public Action<int> OnEnemyEnter;

    private void OnTriggerEnter(Collider other){
        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy == null)
            return;

        OnEnemyEnter?.Invoke(enemy.damage);
        Destroy(enemy.gameObject);
    }



}
