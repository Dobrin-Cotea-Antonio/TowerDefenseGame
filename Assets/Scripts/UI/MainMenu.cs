using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Class <c>MainMenu</c> is used for the buttons in the main menu.
/// </summary>
public class MainMenu : MonoBehaviour{

    public void Quit() {
        Application.Quit();
    }

    public void LoadLevel(string pTargetLevel) {
        SceneManager.LoadScene(pTargetLevel);
    }

}
