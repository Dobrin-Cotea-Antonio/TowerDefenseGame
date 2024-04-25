using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface <c>IAttacker</c> can be implemented to create an attacker to be used by the towers.
/// </summary>
public interface IAttacker{

    public void Attack(Transform pTarget, float pProjectileSpeed,int pProjectileDamage,float pProjectileRange); 

}
