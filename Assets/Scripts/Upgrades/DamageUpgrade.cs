using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class <c>DamageUpgrade</c> is used to upgrade the damage of a tower and change its visual aspect.
/// </summary>
public class DamageUpgrade : Upgrade{

    [SerializeField] int upgradeAmount;

    [Header("UI Info")]
    [SerializeField] string descriptionText;
    [SerializeField] string upgradeNameText;

    public override void ApplyUpgrade() {
        targetTower.damage += upgradeAmount;
        ApplyMaterials();
    }

    public override void FormatDescription(TextMeshProUGUI pText) {
        pText.text = descriptionText + upgradeAmount.ToString();

    }
    public override void FormatUpgradeText(TextMeshProUGUI pText) {
        pText.text = upgradeNameText;
    }
}
