using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface <c>IDamagable</c> can be implemented for an object that needs to take damage and have status effects.
/// </summary>
public interface IDamagable{
    public void TakeDamage(int pDamage);

    public void ApplyStatusEffect(GameObject pStatusEffectPrefab);
}
