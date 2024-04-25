using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Class <c>FinalMenu</c> is used to control the end screen of the game.
/// </summary>
public class FinalMenu : MonoBehaviour{
    public static FinalMenu instance { get; private set; }

    [SerializeField] GameObject menu;
    [SerializeField] TextMeshProUGUI text;

    private void Awake(){
        if (instance != null)
            Destroy(instance);
        instance = this;
    }


    public void EndGame(bool pWon) {
        Time.timeScale = 0;

        menu.SetActive(true);

        if (pWon)
            text.text = "YOU WON!";
        else
            text.text = "YOU LOST!";

    }

    public void LoadLevel(string pTarget){
        Time.timeScale = 1;
        SceneManager.LoadScene(pTarget);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
