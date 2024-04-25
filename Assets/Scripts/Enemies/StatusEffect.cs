using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// Class <c>StatusEffect</c> is a base class that can be used to create status effects that are applied to enemies on hit.
/// </summary>
public abstract class StatusEffect : MonoBehaviour {

    public Action<StatusEffect> OnDestroy;

    [SerializeField] protected StatusEffectData data;

    protected float timeElapsed = 0;
    protected EnemyController target;
    protected int stackCount = 0;

    public abstract void Apply();

    protected abstract void Remove();

    public abstract void StackEffect();

    public void SetTarget(EnemyController pTarget) {
        target = pTarget;
        OnSetTarget();
    }

    protected abstract void OnSetTarget();

    public Sprite GetImage() {
        return data.image;
    }

    public float ReturnTimeLeft() {
        return data.duration - timeElapsed;
    }
}
