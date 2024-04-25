using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// Class <c>AttackSpeedUpgrade</c> is used to upgrade the attack speed of a tower and change its visual aspect.
/// </summary>
public class AttackSpeedUpgrade : Upgrade{
    [SerializeField] float upgradeAmount;

    [Header("UI Info")]
    [SerializeField] string descriptionText;
    [SerializeField] string upgradeNameText;

    public override void ApplyUpgrade()
    {
        targetTower.attackCooldown -= upgradeAmount;
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
