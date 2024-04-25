using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>TowerTargets</c> is used to choose the target of a tower.
/// </summary>
public class TowerTargets : MonoBehaviour {

    public List<Transform> enemiesInRange { get; private set; }

    private void Awake() {
        enemiesInRange = new List<Transform>();
    }

    private void Update() {
        ClearEmptyPosition();
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Enemy"))
            return;

        enemiesInRange.Add(other.transform);
    }

    private void OnTriggerExit(Collider other) {

        if (!other.CompareTag("Enemy"))
            return;

        enemiesInRange.Remove(other.transform);

    }

    void ClearEmptyPosition() {
        for (int i = 0; i < enemiesInRange.Count; i++)
            if (enemiesInRange[i] == null)
                enemiesInRange.RemoveAt(i);
    }


    public Transform ReturnFastestEnemy() {

        ClearEmptyPosition();

        float maxSpeed = -float.MaxValue;
        Transform targetTransform = null;

        foreach (Transform t in enemiesInRange) {
            EnemyData data = t.GetComponent<EnemyController>().data;

            if (data.speed > maxSpeed) {
                maxSpeed = data.speed;
                targetTransform = t;
            }

        }

        return targetTransform;

    }

    //return the first enemy in the list(the one that entered first) with the maximum hp
    public Transform ReturnTankiestEnemy() {

        ClearEmptyPosition();

        float maxHP = -float.MaxValue;
        Transform targetTransform = null;

        foreach (Transform t in enemiesInRange) {
            EnemyData data = t.GetComponent<EnemyController>().data;

            if (data.maxEnemyHP > maxHP) {
                maxHP = data.maxEnemyHP;
                targetTransform = t;
            }

        }

        return targetTransform;

    }

    public Transform ReturnClosestEnemy() {

        ClearEmptyPosition();

        float distance = float.MaxValue;
        Transform targetTransform = null;

        foreach (Transform t in enemiesInRange){

            float dis = (t.position - transform.position).magnitude;

            if (dis < distance){
                distance = dis;
                targetTransform = t;
            }

        }

        return targetTransform;


    }

    public Transform ReturnFurthestEnemy() {

        ClearEmptyPosition();

        float distance = -float.MaxValue;
        Transform targetTransform = null;

        foreach (Transform t in enemiesInRange){

            float dis = (t.position - transform.position).magnitude;

            if (dis > distance){
                distance = dis;
                targetTransform = t;
            }

        }

        return targetTransform;

    }

}
