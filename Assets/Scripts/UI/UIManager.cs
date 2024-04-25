using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;


/// <summary>
/// Class <c>TowerButton</c> is used to store data about the buttons used to buy towers.
/// </summary>
[System.Serializable]
public class TowerButton {

    public Button button;
    public TowerData data;
    public TextMeshProUGUI text;
    public Image image;
    public Image towerImage;

}

/// <summary>
/// Class <c>UpgradeButton</c> is used to store data about the upgrade buttons.
/// </summary>
[System.Serializable]
public class UpgradeButton {

    public int upgradeRowIndex;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI upgradeNameText;
    public Image backgroundImage;
    public Image[] upgradeLevelIndicators;
    public Button upgradeButton;
    public RectTransform descriptionTransform;
}


/// <summary>
/// Class <c>UiManager</c> is used to manage all the gameplay related UI.
/// </summary>
public class UIManager : MonoBehaviour {

    public static UIManager uiManager { get; private set; }

    [Header("Round Text")]
    [SerializeField] TextMeshProUGUI roundText;

    [Header("Money Text")]
    [SerializeField] TextMeshProUGUI moneyText;

    [Header("HP")]
    [SerializeField] TextMeshProUGUI hpText;

    [Header("Round Info Text")]
    [SerializeField] TextMeshProUGUI roundInfoText;
    [SerializeField] TextMeshProUGUI roundInfoNumber;
    [SerializeField] string waveDurationText;
    [SerializeField] string waveEnemyCountText;
    [SerializeField] string waveStartText;

    [Header("Tower Shop")]
    [SerializeField] RectTransform panel;
    [SerializeField] Button shopButton;
    [SerializeField] TowerButton[] towerButtons;
    [SerializeField] float animationTime;
    [SerializeField] Color backgroundColorCanBuy;
    [SerializeField] Color backgroundColorCantBuy;
    [SerializeField] Color textColorCanBuy;
    [SerializeField] Color textColorCantBuy;

    bool shopIsVisible = true;

    [Header("Tower Upgrades")]
    [SerializeField] GameObject upgradeHolder;
    [SerializeField] TextMeshProUGUI sellText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] UpgradeButton[] upgradeButtons;
    [SerializeField] Image towerImage;
    [SerializeField] RectTransform descriptionPage;
    [SerializeField] Vector2 descriptionTweenDistance;
    [SerializeField] float desctiptionTweenDuration;

    [SerializeField] TextMeshProUGUI targetingTypeText;

    TowerController selectedTowerForUpgrades;

    [Header("Tower Info")]
    [SerializeField] GameObject towerDescriptionPage;
    [SerializeField] TextMeshProUGUI descriptionText;


    int lastEnemyAmount = 0;

    private void Awake(){
        if (uiManager != null)
            Destroy(uiManager);

        uiManager = this;
    }

    private void Start(){
        foreach (TowerButton t in towerButtons) {
            t.text.text = string.Format("{0}",t.data.cost);
            t.towerImage.sprite = t.data.sprite;
        }
    }

    private void Update(){
        UpdateShopButtons();
        UpdateUpgradePanel();
    }

    public void SetRoundText(int pCurrentRound,int pRoundsMax) {
        roundText.text = string.Format("{0}/{1}",pCurrentRound+1,pRoundsMax);

    }

    public void SetTimeScale(float pScale) {
        Time.timeScale = pScale;
    }

    public void DisplayWaveDuration(float pDuration) {
        roundInfoText.text = waveDurationText;
        StartCoroutine(WaveDurationCoroutine(pDuration));
    }

    public void DisplayWaveStartText(float pDuration) {
        roundInfoText.text = waveStartText;
        roundInfoNumber.text = string.Format("{0}",pDuration);
        StartCoroutine(WaveDurationCoroutine(pDuration));
        //start a coroutine with an effect and change the color/font size
    }

    public void DisplayEnemyCount(int pAmount) {
        if (pAmount != lastEnemyAmount) {
            roundInfoText.text = waveEnemyCountText;
            roundInfoNumber.text = string.Format("{0}",pAmount);
            lastEnemyAmount = pAmount;
            //add an effect/animation here
        }
    }

    IEnumerator WaveDurationCoroutine(float pDuration) {

        float pTime=0;

        while (pTime < pDuration) {
            roundInfoNumber.text = string.Format("{0}", (int)(pDuration - pTime));
            pTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
    
    }

    public void MoveShop() {
        int value = 0;

        if (shopIsVisible)
            value = 1;
        else
            value = -1;

        shopIsVisible = !shopIsVisible;

        EnableButtons(false);

        Vector2 dist = new Vector2(0, -panel.rect.height);

        Tween t = DOTween.To(() => panel.anchoredPosition, x => panel.anchoredPosition = x, panel.anchoredPosition + value * dist, animationTime);
        t.OnComplete(() => EnableButtons(true));
        t.SetUpdate(UpdateType.Normal, true);

    }

    void TweenUpgradeDescription(RectTransform pTarget,Vector2 pDistance,float pTime,bool pState) {
        int value = 0;
        Tween t;

        if (pState){
            value = 1;
            EnableUpgradeDescription(pTarget, pState);
        }

        else {
            value = -1;
        }

        pDistance = new Vector2(pDistance.x*descriptionPage.rect.x,pDistance.y* descriptionPage.rect.y);

        t = DOTween.To(() => pTarget.anchoredPosition, x => pTarget.anchoredPosition = x, pTarget.anchoredPosition + value * pDistance, pTime);
        if (!pState)
            t.OnComplete(() => EnableUpgradeDescription(pTarget, pState));
        t.SetUpdate(UpdateType.Normal, true);
    }

    void EnableUpgradeDescription(RectTransform pTarget,bool pState) {
        pTarget.gameObject.SetActive(pState);
    }

    void EnableButtons(bool pState) {
        shopButton.enabled = pState;

        foreach (TowerButton t in towerButtons) 
            t.button.enabled = pState;

    }

    public void SetTowerIndicator(TowerData pData){
        Camera.main.GetComponent<CameraInteraction>().SetTowerIndicator(pData);
    }

    public void MarkButtonAsClicked(){
        Camera.main.GetComponent<CameraInteraction>().MarkButtonAsClicked();
    }


    public void UpdateMoneyCounter(int pMoney) {
        moneyText.text = string.Format("{0}", pMoney);
    }

    void UpdateShopButtons() {
        LevelManager levelManager = LevelManager.levelManager;
        foreach (TowerButton t in towerButtons) {
            if (t.data.cost > levelManager.money){
                t.button.enabled = false;
                t.text.color = textColorCantBuy;
                t.image.color = backgroundColorCantBuy;
            }
            else {
                t.button.enabled = true;
                t.text.color = textColorCanBuy;
                t.image.color = backgroundColorCanBuy;
            }
        
        }
    }

    public void EnableUpgradeTab(GameObject pSelectedTower) {
        if (selectedTowerForUpgrades!=null)
            selectedTowerForUpgrades.GetComponent<TowerController>().ChangeRangeIndicatorState(false);

        upgradeHolder.SetActive(true);
        selectedTowerForUpgrades = pSelectedTower.GetComponent<TowerController>();
        selectedTowerForUpgrades.GetComponent<TowerController>().ChangeRangeIndicatorState(true);
    }

    public void DisableUpgradeTab() {
        if (selectedTowerForUpgrades != null)
            selectedTowerForUpgrades.GetComponent<TowerController>().ChangeRangeIndicatorState(false);

        upgradeHolder.SetActive(false);
        selectedTowerForUpgrades = null;
    }

    public void UpgradeSelectedTower(int pIndex){

        Upgrade upgrade = selectedTowerForUpgrades.GetNextUpgrade(pIndex);
        if (upgrade == null)
            return;

        if (upgrade.upgradeCost > LevelManager.levelManager.money)
            return;

        
        selectedTowerForUpgrades.IncreaseUpgradeLevel(pIndex);
        LevelManager.levelManager.AddMoney(-upgrade.upgradeCost);

    }

    public void SellSelectedTower() {
        LevelManager.levelManager.AddMoney(selectedTowerForUpgrades.ReturnTowerValue());
        Destroy(selectedTowerForUpgrades.gameObject);
        DisableUpgradeTab();
    }

    void UpdateUpgradePanel() {
        if (selectedTowerForUpgrades == null)
            return;

        //update the name
        nameText.text = string.Format("{0}",selectedTowerForUpgrades.data.towerName);
        //update the sell value
        sellText.text = string.Format("{0}", selectedTowerForUpgrades.ReturnTowerValue());

        towerImage.sprite = selectedTowerForUpgrades.data.sprite;

        //update targeting type
        targetingTypeText.text = selectedTowerForUpgrades.targetingType.ToString();

        foreach (UpgradeButton button in upgradeButtons){

            //update upgrade names
            Upgrade currentUpgrade = selectedTowerForUpgrades.GetCurrentUpgrade(button.upgradeRowIndex);
            currentUpgrade.FormatDescription(button.descriptionText);
            currentUpgrade.FormatUpgradeText(button.upgradeNameText);

            //update level indicators
            for (int i = 0; i < button.upgradeLevelIndicators.Length; i++)
                if (selectedTowerForUpgrades.GetUpgradeLevel(button.upgradeRowIndex) <= i)
                    button.upgradeLevelIndicators[i].color = Color.red;
                else
                    button.upgradeLevelIndicators[i].color = Color.green;

            Upgrade nextUpgrade= selectedTowerForUpgrades.GetNextUpgrade(button.upgradeRowIndex);

            //if the upgrade is max level disable it and check the next button
            if (nextUpgrade == null) {
                button.upgradeButton.enabled = false;
                continue;
            }

            //enable button
            button.upgradeButton.enabled = true;

            //update the price
            button.priceText.text = string.Format("{0}", nextUpgrade.upgradeCost);

            //update text and background color to be able to easily see if you can afford something

            if (nextUpgrade.upgradeCost > LevelManager.levelManager.money){
                button.priceText.color = textColorCantBuy;
                button.backgroundImage.color = backgroundColorCantBuy;
            }
            else{
                button.priceText.color = textColorCanBuy;
                button.backgroundImage.color = backgroundColorCanBuy;
            }

        }

    }

    public void EnableUpgradeDescription(int pUpgradeButtonIndex) {
        if (!upgradeButtons[pUpgradeButtonIndex].upgradeButton.enabled)
            return;

        TweenUpgradeDescription(upgradeButtons[pUpgradeButtonIndex].descriptionTransform, descriptionTweenDistance, desctiptionTweenDuration, true);
    }

    public void DisableUpgradeDescription(int pUpgradeButtonIndex){
        TweenUpgradeDescription(upgradeButtons[pUpgradeButtonIndex].descriptionTransform, descriptionTweenDistance, desctiptionTweenDuration, false);
    }

    public void CycleTargetingType(bool pForward) {
        selectedTowerForUpgrades.CycleTargetingType(pForward);
    }

    public void UpdateHp(int pHp) {
        hpText.text = pHp.ToString();
    }

    //returns a formated description for a tower that will be displayed on the ui contained all the necessary info
    string FormatDescriptionText(TowerData pData) {

        string finalString=null;
        finalString = string.Format("Description: {0}\nDamage: {1}\nAttack Cooldown: {2}\nRange: {3}",pData.description,pData.attackDamage,pData.attackCooldown,pData.attackRange );

        string str = null;
        if (pData.AOESize == 0)
            str = "Single";
        else
            str = "AOE";
        finalString += "\nAttack Type: "+str;

        return finalString;
    }

    public void EnableTowerDescription(TowerData pTowerData){
        towerDescriptionPage.SetActive(true);
        descriptionText.text = FormatDescriptionText(pTowerData);
    }

    public void DisableTowerDescription() {
        towerDescriptionPage.SetActive(false);
    
    }


}
