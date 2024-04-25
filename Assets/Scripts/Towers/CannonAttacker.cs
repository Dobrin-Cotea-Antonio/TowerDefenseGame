using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>CannonAttacker</c> is used to shoot a cannon ball that stuns the enemy. It should be used with the cannon tower.
/// </summary>
public class CannonAttacker : MonoBehaviour, IAttacker{

    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject ballPrefab;

    [SerializeField] List<GameObject> statusEffectsPrefabs;

    public void Attack(Transform pTarget, float pProjectileSpeed, int pProjectileDamage, float pProjectileRange){

        GameObject g = Instantiate(ballPrefab, shootPoint.position, shootPoint.rotation);
        Projectile projectile = g.GetComponent<Projectile>();

        projectile.StartProjectile(pProjectileSpeed, pProjectileDamage, pProjectileRange);
        projectile.OnEnemyImpact += ImpactEffect;
    }

    void ImpactEffect(IDamagable pEnemy) {
        foreach (GameObject g in statusEffectsPrefabs){
            pEnemy.ApplyStatusEffect(g);
        }
    }

}
