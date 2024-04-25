using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>CheatMenu</c> provides various cheats that can be used to test the game.
/// </summary>
public class CheatMenu : MonoBehaviour {

    public static CheatMenu instance { get; private set; }

    public bool infiniteMoney;
    public bool infiniteBaseHp;
    public bool instantEnemyKill;
    public bool finishLevel;

    private void Awake(){
        if (instance != null)
            Destroy(instance);
        instance = this;
    }

}
