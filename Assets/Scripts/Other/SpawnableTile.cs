using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class <c>SpawnableTile</c> is placed on an object that the towers can be placed on.
/// </summary>
public class SpawnableTile : MonoBehaviour{

    [SerializeField] Transform towerSpawnPoint;
    [SerializeField] bool hasObstacles;

    GameObject tower;

    public void PlaceTower(TowerData pData) {
        tower = Instantiate(pData.towerPrefab, towerSpawnPoint.position, Quaternion.identity);
    }

    public bool IsTileOccupied() {
        return (tower != null|| hasObstacles);
    }

    public void DestroyTower(){

        Destroy(tower);
        tower = null;

    }

}
