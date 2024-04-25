using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// Class <c>MoneyDropText</c> is used to display the cash dropped on an enemy`s death and manage its lifetime.
/// </summary>
public class MoneyDropText : MonoBehaviour{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float lifeTime;

    public void SetText(string pText) {
        text.text = pText;
        Destroy(this.gameObject, lifeTime);
    }
}
