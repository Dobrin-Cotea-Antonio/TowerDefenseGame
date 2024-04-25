using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;


/// <summary>
/// Class <c>CameraInteraction</c> is used place towers and detect when the player clicks on a tower int he scene.
/// </summary>
public class CameraInteraction : MonoBehaviour{

    public static CameraInteraction cameraInteraction { get; private set; }

    [Header("Tower Indicator")]
    [SerializeField] LayerMask mask;
    [SerializeField] LayerMask placebleMask;
    [SerializeField] Material materialClear;
    [SerializeField] Material materialObstructed;

    GameObject towerIndicator;
    TowerData towerData;
    SpawnableTile tile;

    [Header("Tower Upgrades")]
    [SerializeField] LayerMask towerMask;


    GameObject targetTower;

    bool buttonWasClicked=false;

    private void Awake(){
        cameraInteraction = this;
    }

    private void Update(){

        CastRayIndicator();
        CastRayTower();

    }

    void CastRayIndicator(){

        if (towerIndicator == null)
            return;

        Ray cameraRay;
        RaycastHit cameraRayHit;

        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out cameraRayHit, Mathf.Infinity, mask)){
            towerIndicator.transform.position = cameraRayHit.point;

            Renderer renderer = towerIndicator.GetComponent<Renderer>();

            if (placebleMask == (placebleMask | (1 << cameraRayHit.collider.gameObject.layer))){

                tile = cameraRayHit.collider.GetComponent<SpawnableTile>();
                if (!tile.IsTileOccupied()){
                    renderer.material = materialClear;
                    return;
                }

                renderer.material = materialObstructed;

            } else {
                renderer.material = materialObstructed;
                tile = null;
            }

        }

    }

    void CastRayTower() {

        Ray cameraRay;
        RaycastHit cameraRayHit;

        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraRay, out cameraRayHit, Mathf.Infinity, towerMask)) {
            targetTower = cameraRayHit.collider.gameObject;
            return;
        }


        targetTower = null;

    }

    public void SetTowerIndicator(TowerData pData) {

        if (!buttonWasClicked)
            return;


        ClearTowerIndicator();
        buttonWasClicked = false;
        towerIndicator = Instantiate(pData.hoverPrefab);
        towerData = pData;
    }

    public void MarkButtonAsClicked() {
        buttonWasClicked = true;
    }

    void ClearTowerIndicator() {
        if (towerIndicator != null) {
            buttonWasClicked = false;
            Destroy(towerIndicator);
            towerIndicator = null;
            towerData = null;
        }
    }

    void PlaceTower() {
        if (tile.IsTileOccupied())
            return;

        if (LevelManager.levelManager.money < towerData.cost)
            return;

        LevelManager.levelManager.AddMoney(-towerData.cost);
        tile.PlaceTower(towerData);
        ClearTowerIndicator();
        
    }

    void OnRightClick() {
        ClearTowerIndicator();
        UIManager.uiManager.DisableUpgradeTab();
    }

    void OnLeftClick() {

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (targetTower != null){
            UIManager.uiManager.EnableUpgradeTab(targetTower);
            return;
        }

        if (towerIndicator == null || towerData == null || tile == null)
            return;

        PlaceTower();

    }

}
