using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>CatapultAttacker</c> is used to launch an AOE projectile at enemies. It should be used with the catapult tower.
/// </summary>
public class CatapultAttacker : MonoBehaviour, IAttacker {
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject explosionPrefab;


    [SerializeField] List<GameObject> statusEffectsPrefabs;


    TowerController tower;
    TowerData data;

    Transform target;
    float projectileSpeed;
    int projectileDamage;
    float projectileRange;

    Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        tower = GetComponent<TowerController>();
        data = tower.data;
    }

    public void Attack(Transform pTarget, float pProjectileSpeed, int pProjectileDamage, float pProjectileRange) {

        target = pTarget;
        projectileSpeed = pProjectileSpeed;
        projectileDamage = pProjectileDamage;
        projectileRange = pProjectileRange;

        if (animator != null) {
            animator.SetTrigger("Shoot");
            return;
        }

        SpawnProjectile();

    }

    void SpawnProjectile() {
        //target gets destroyed so it breaks?
        if (target == null) {
            target = tower.RequestTarget();
            if (target == null)
                return;
        }

        GameObject g = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);

        g.transform.forward = (target.position - shootPoint.position).normalized;
        Projectile projectile = g.GetComponent<Projectile>();

        projectile.StartProjectile(projectileSpeed, projectileDamage, projectileRange);
        projectile.OnEnemyImpact += ImpactEffect;
        projectile.OnImpact += AOEAttack;
    }

    void ImpactEffect(IDamagable pEnemy) {
        foreach (GameObject g in statusEffectsPrefabs) {
            pEnemy.ApplyStatusEffect(g);
        }
    }

    void AOEAttack(Vector3 pPos) {

        GameObject g = Instantiate(explosionPrefab, pPos, Quaternion.identity);
        g.transform.localScale = new Vector3(data.AOESize, data.AOESize, data.AOESize);
        Explosion explosion = g.GetComponent<Explosion>();
        explosion.damage = projectileDamage;
        Destroy(g, 0.075f);

    }
}
