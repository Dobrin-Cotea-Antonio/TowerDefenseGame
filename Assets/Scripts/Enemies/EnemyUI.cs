using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class <c>EnemyUI</c> is used to display the HP of enemies and their active status effects.
/// </summary>
[RequireComponent(typeof(EnemyController))]
public class EnemyUI : MonoBehaviour{

    [SerializeField] RectTransform hpBarTransform;

    [SerializeField] GameObject statusEffectPage;

    [SerializeField] GameObject statusEffectPrefab;
    Dictionary<StatusEffect, StatusEffectDisplay> statusEffects=new Dictionary<StatusEffect, StatusEffectDisplay>();


    private void Start(){
        GetComponent<EnemyController>().OnDamageTaken += UpdateHPBar;
    }

    private void OnEnable(){
        EnemyController enemy = GetComponent<EnemyController>();
        enemy.OnStatusCreate += AddStatusEffect;
        enemy.OnStatusRemove += RemoveStatusEffect;
    }

    private void OnDisable(){
        EnemyController enemy = GetComponent<EnemyController>();
        enemy.OnStatusCreate -= AddStatusEffect;
        enemy.OnStatusRemove -= RemoveStatusEffect;
    }

    void UpdateHPBar(int pCurrentHP,int pMaxHP) {
        float percentage = 1.0f * pCurrentHP / pMaxHP;

        hpBarTransform.localScale = new Vector3(percentage, 1, 1);
    }

    void AddStatusEffect(StatusEffect pStatus) {
        if (pStatus == null)
            return;

        GameObject prefab = Instantiate(statusEffectPrefab, statusEffectPage.transform);
        StatusEffectDisplay s = prefab.GetComponent<StatusEffectDisplay>();
        statusEffects[pStatus] = s;
        s.UpdateImage(pStatus.GetImage());
    }

    void RemoveStatusEffect(StatusEffect pStatus) {
        if (pStatus == null)
            return;

        Destroy(statusEffects[pStatus].gameObject);
        statusEffects.Remove(pStatus);
    }

    private void Update(){
        foreach (KeyValuePair<StatusEffect, StatusEffectDisplay> pair in statusEffects){
            pair.Value.UpdateText(System.Math.Round(pair.Key.ReturnTimeLeft(),2).ToString());
        }

        
    }
    

}
