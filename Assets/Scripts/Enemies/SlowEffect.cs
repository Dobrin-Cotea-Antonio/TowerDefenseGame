using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>SlowEffect</c> inherits from the StatusEffect class. It slows down enemies.
/// </summary>
public class SlowEffect : StatusEffect {

    float initialSpeedValue=0;

    public override void Apply(){
        if (target == null) {
            Remove();
            return;
        }

        stackCount++;
        target.speed = (1-data.value/100) * initialSpeedValue;
        //Debug.Log(target.speed+" "+initialSpeedValue);
    }

    protected override void Remove() {

        target.speed = initialSpeedValue;
        OnDestroy?.Invoke(this);
        Destroy(this.gameObject);
    }

    private void Update(){
        if (target == null)
            return;

        timeElapsed += Time.deltaTime;
        if (timeElapsed > data.duration){
            Remove();
        }
    }

    public override void StackEffect() {

        if (stackCount == 0) 
            return;
        

        if (data.canStack){
            ResetEffect();
            Apply();
        } else {
            timeElapsed = 0;
        }

            

    }

    protected void ResetEffect(){
        target.speed = initialSpeedValue;
        timeElapsed = 0;
    }

    protected override void OnSetTarget(){
        initialSpeedValue = target.speed;
    }
}
