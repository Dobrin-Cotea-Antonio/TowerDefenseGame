using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Explosion</c> is used to detect enemies for an AOE attack and damage them.
/// </summary>
public class Explosion : MonoBehaviour {

    [SerializeField] GameObject explosionVisual;

    public int damage { get; set; }

    private void Start() {
        GameObject e = Instantiate(explosionVisual, transform.position, Quaternion.identity);
        Destroy(e, 1.5f);
    }


    private void OnTriggerEnter(Collider other){

        IDamagable damagableEnemy = other.gameObject.GetComponent<IDamagable>();

        if (damagableEnemy != null){
            damagableEnemy.TakeDamage(damage);
        }

    }


}
