using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Class <c>PauseMenu</c> is used to manage the pause menu screen.
/// </summary>
public class PauseMenu : MonoBehaviour{

    public static PauseMenu instance { get; private set; }

    [SerializeField] GameObject pauseMenu;
    bool isGamePaused = false;
    float previousTimeScale=1;

    private void Awake(){
        if (instance != null)
            Destroy(instance);
        instance = this;
    }

    private void OnPause(){
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        isGamePaused = !isGamePaused;
        Time.timeScale = (isGamePaused) ? 0 : previousTimeScale;
    }

    private void Update(){
        if (Time.timeScale != 0)
            previousTimeScale = Time.timeScale;
    }

    public void QuitToDesktop(){
        Application.Quit();
    }

    public void LoadLevel(string pTarget) {
        Time.timeScale = 1;
        SceneManager.LoadScene(pTarget);
    }

    public void Resume() {
        OnPause();
    }

    public void RestartLevel() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DisablePage() {
        gameObject.SetActive(false);
    }
}
