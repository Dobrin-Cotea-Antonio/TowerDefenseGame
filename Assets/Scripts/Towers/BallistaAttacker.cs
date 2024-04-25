using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class <c>Ballista Attacker</c> is used to fire a single bolt towards an enemy. It should be used on the ballista tower.
/// </summary>
public class BallistaAttacker : MonoBehaviour, IAttacker {

    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject arrowPrefab;

    public void Attack(Transform pTarget, float pProjectileSpeed, int pProjectileDamage, float pProjectileRange) {

        GameObject g = Instantiate(arrowPrefab,shootPoint.position,shootPoint.rotation);
        Projectile projectile = g.GetComponent<Projectile>();

        projectile.StartProjectile(pProjectileSpeed,pProjectileDamage,pProjectileRange);
        
    }

}
