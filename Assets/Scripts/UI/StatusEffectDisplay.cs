using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Class <c>StatusEffectDisplay</c> is used to display the status effect of an enemy(update text and image) and manage its lifetime.
/// </summary>
public class StatusEffectDisplay : MonoBehaviour{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;


    public void UpdateImage(Sprite pImage){
        image.sprite = pImage;
    }

    public void UpdateText(string pText) {
        text.text = pText;
    }

    public void DestroyStatusEffectDisplay() {
        Destroy(this.gameObject);
    }

}
