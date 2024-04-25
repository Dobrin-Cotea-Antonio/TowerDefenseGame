using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class <c>RangeUpgrade</c> is used to upgrade the range of a tower and change its visual aspect.
/// </summary>
public class RangeUpgrade : Upgrade{
    [SerializeField] int upgradeAmount;

    [Header("UI Info")]
    [SerializeField] string descriptionText;
    [SerializeField] string upgradeNameText;

    public override void ApplyUpgrade(){
        targetTower.range += upgradeAmount;
        ApplyMaterials();
    }

    public override void FormatDescription(TextMeshProUGUI pText)
    {
        pText.text = descriptionText + upgradeAmount.ToString();

    }
    public override void FormatUpgradeText(TextMeshProUGUI pText)
    {
        pText.text = upgradeNameText;
    }
}
