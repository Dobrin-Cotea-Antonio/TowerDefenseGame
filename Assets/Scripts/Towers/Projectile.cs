using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class <c>Projectile</c> is shot by the towers. It is used to move the projectile and damage the enemies.
/// </summary>
public class Projectile : MonoBehaviour{

    public Action<IDamagable> OnEnemyImpact;
    public Action<Vector3> OnImpact;

    Vector3 startPosition;
    float range=10000;
    int damage;
    bool wasStarted = false;

    Rigidbody rb;

    void Update(){
        if (!wasStarted)
            return;

        if ((transform.position - startPosition).magnitude > 200)
            Destroy(this);
    }

    public void StartProjectile(float pProjectileSpeed, int pProjectileDamage, float pProjectileRange){

        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward*pProjectileSpeed, ForceMode.VelocityChange);
        damage = pProjectileDamage;
        range = pProjectileRange;
        startPosition = transform.position;

        wasStarted = true;
    }

    private void OnCollisionEnter(Collision collision){

        IDamagable damagableEnemy = collision.gameObject.GetComponent<IDamagable>();

        if (damagableEnemy!=null) {
            damagableEnemy.TakeDamage(damage);
            OnEnemyImpact?.Invoke(damagableEnemy);
        }

        OnImpact?.Invoke(transform.position);
        Destroy(gameObject);
        
    }
}
