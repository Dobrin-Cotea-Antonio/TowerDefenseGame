using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes { 
    Goblin,
    WolfRider
}


/// <summary>
/// Class <c>EnemyInfo</c> is used to store data about what enemy types can spawn in a level.
/// </summary>
[System.Serializable]
public class EnemyInfo {

    [SerializeField] EnemyTypes _type;
    [SerializeField] float _chanceToPick;

    public EnemyTypes type { get { return _type; } }
    public float chanceToPick { get { return _chanceToPick; } }
}


/// <summary>
/// Class <c>WaveMovement</c> is used to store data about each wave that takes place in a level.
/// </summary>
[System.Serializable]
public class WaveData {

    [SerializeField] EnemyInfo[] _enemyList;
    [SerializeField] float _waveDuration;
    [SerializeField] float _waveCooldown;
    [SerializeField] float _activeSpawnerCount;
    [SerializeField] float _enemySpawnDelay;

    public EnemyInfo[] enemyList { get { return _enemyList; } }
    public float waveDuration { get { return _waveDuration; } }
    public float waveCooldown { get { return _waveCooldown; } }
    public float activeSpawnerCount { get { return _activeSpawnerCount; } }
    public float enemySpawnDelay { get { return _enemySpawnDelay; } }
}

/// <summary>
/// Class <c>LevelManager</c> is used to start new waves, spawn enemies, detect when a new wave can spawn and detect when the level is over.
/// </summary>
public class LevelManager : MonoBehaviour {

    public static LevelManager levelManager;

    [Header("Enemy Prefabs")]
    [SerializeField] GameObject[] enemyPrefabs;

    [Header("Wave Data")]
    [SerializeField] WaveData[] waves;
    int currentWave = -1;

    [Header("Spawn Points")]
    [SerializeField] Waypoint[] _spawnPoints;//do not touch
    List<Waypoint> spawnPoints;
    List<Waypoint> activeSpawnPoints = new List<Waypoint>();

    [Header("Target")]
    [SerializeField] Waypoint _endPoint;

    [Header("Starting Money")]
    [SerializeField] int startingMoney;

    [Header("Starting Lives")]
    [SerializeField] int lives;

    Coroutine enemySpawnCoroutine;
    public int money { get; private set; }

    bool waveIsActive = false;
    UIManager uiManager;

    int enemyCount = 0;

    private void Awake() {
        if (levelManager != null)
            Destroy(levelManager);

        levelManager = this;
    }

    private void Start() {
        uiManager = UIManager.uiManager;
        ChooseActiveSpawnPoints();
        AddMoney(startingMoney);
        UIManager.uiManager.UpdateHp(lives);

        EndPoint[] endPoints = FindObjectsOfType<EndPoint>();
        foreach (EndPoint endPoint in endPoints) {
            endPoint.OnEnemyEnter += DecreaseLives;
        }
        
    }

    private void Update() {
        CheatMenu cheatMenu = CheatMenu.instance;

        if (cheatMenu != null) {
            if (cheatMenu.infiniteMoney)
                AddMoney(9999-money);
            if (cheatMenu.infiniteBaseHp) {
                lives = 9999;
                UIManager.uiManager.UpdateHp(lives);
            }

            if (cheatMenu.finishLevel) {
                UIManager.uiManager.gameObject.SetActive(false);
                PauseMenu.instance.DisablePage();
                FinalMenu.instance.EndGame(true);
            }
        }


        AttemptToEndLevel();

        if (!waveIsActive)
            return;
    }

    void ChooseActiveSpawnPoints() {
        currentWave++;
        if (currentWave == waves.Length) {
            UIManager.uiManager.gameObject.SetActive(false);
            PauseMenu.instance.DisablePage();
            FinalMenu.instance.EndGame(true);
            return;
        }
        uiManager.SetRoundText(currentWave, waves.Length);
        waveIsActive = false;

        spawnPoints = new List<Waypoint>(_spawnPoints);
        for (int i = 0; i < waves[currentWave].activeSpawnerCount; i++) {
            int randomSpawnerIndex = Random.Range(0, spawnPoints.Count);
            activeSpawnPoints.Add(spawnPoints[randomSpawnerIndex]);
            spawnPoints.RemoveAt(randomSpawnerIndex);
        }

        uiManager.DisplayWaveStartText(waves[currentWave].waveCooldown);
        StartCoroutine(WaitForWaveStart());

    }


    IEnumerator WaitForWaveStart() {
        EnableWarningUI(true);

        yield return new WaitForSeconds(waves[currentWave].waveCooldown);

        EnableWarningUI(false);

        waveIsActive = true;
        enemySpawnCoroutine = StartCoroutine(EnemySpawnCoroutine());

        StartCoroutine(WaitForWaveEnd());
    }

    void EnableWarningUI(bool pState) {
        //show a prompt for how long you have left to prepare and some indicators to show where the enemies are coming from
    }


    IEnumerator WaitForWaveEnd() {
        uiManager.DisplayWaveDuration(waves[currentWave].waveDuration);

        yield return new WaitForSeconds(waves[currentWave].waveDuration);

        StopCoroutine(enemySpawnCoroutine);
        StartCoroutine(WaitForEnemiesToDie());
        //ChooseActiveSpawnPoints();

    }

    IEnumerator EnemySpawnCoroutine() {

        while (true) {
            SpawnEnemy();
            yield return new WaitForSeconds(waves[currentWave].enemySpawnDelay);
        }

    }

    IEnumerator WaitForEnemiesToDie() {

        while (true) {
            uiManager.DisplayEnemyCount(enemyCount);

            if (enemyCount <= 0) {
                ChooseActiveSpawnPoints();
                break;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    void SpawnEnemy() {
        int randomEnemySpawner = Random.Range(0, activeSpawnPoints.Count);
        float randomNumber = Random.Range(1f, 100f);

        List<EnemyTypes> enemySpawnTypes = new List<EnemyTypes>();
        List<float> enemySpawnChances = new List<float>();

        int index = 0;

        while (true) {

            if (index >= waves[currentWave].enemyList.Length)
                break;

            EnemyInfo info = waves[currentWave].enemyList[index++];//here

            enemySpawnTypes.Add(info.type);

            if (index == 1)
                enemySpawnChances.Add(info.chanceToPick);
            else
                enemySpawnChances.Add(enemySpawnChances[enemySpawnChances.Count - 1] + info.chanceToPick);

        }

        for (int i = 0; i < enemySpawnTypes.Count; i++) 
            if (randomNumber < enemySpawnChances[i]) {
                GameObject g = Instantiate(enemyPrefabs[(int)enemySpawnTypes[i]],activeSpawnPoints[randomEnemySpawner].transform.position,activeSpawnPoints[randomEnemySpawner].transform.rotation);
                enemyCount++;
                EnemyController enemy = g.GetComponent<EnemyController>();
                enemy.OnEnemyDeath += DecreaseEnemyCount;
                enemy.OnEnemyDeath += AddMoney;
                enemy.SetTargetWaypoint(activeSpawnPoints[randomEnemySpawner]);
                break;
            }

    }

    void DecreaseEnemyCount(int pSomething=1) {
        enemyCount--;
        //Debug.Log(enemyCount);
    }

    public void AddMoney(int pAmount) {
        money += pAmount;
        uiManager.UpdateMoneyCounter(money);
    }

    public void DecreaseLives(int pDamage) {
        lives = Mathf.Max(lives - pDamage, 0);
        DecreaseEnemyCount();
        UIManager.uiManager.UpdateHp(lives);
        if (lives == 0) {
            Debug.Log("dead");
        }
    }

    void AttemptToEndLevel(){
        if (lives == 0) {
            UIManager.uiManager.gameObject.SetActive(false);
            PauseMenu.instance.DisablePage();
            FinalMenu.instance.EndGame(false);
        }
    }

}
