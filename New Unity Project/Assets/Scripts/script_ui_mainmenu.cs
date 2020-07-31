using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class script_ui_mainmenu : MonoBehaviour
{
    public bool isPaused;
    public GameObject pauseMenuUI;
    public GameObject endMenuUI;
    public GameObject panelUI;
    public bool isFinished = false;
    //GameObject go = GameObject.Find("prefab_player");
    //Script cs = go.GetComponent<script_player_move>();

    // Start is called before the first frame update
    void Start()
    {

}

    // Update is called once per frame
    void Update(){
        //isFinished = go.GetComponent<script_player_move>().finished;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                activateMenu();
            }
            else
            {
                deactivateMenu();
            }
        }
        if (isFinished)
        {
            activateEndMenu();
        }
    }

    //switch from menu scene to game scene
    public void PlayGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //when button is clicked end the game
    public void QuitGame(){
        #if UNITY_EDITOR
                 UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    //when button is clicked resart the game scene
    public void RestartGame(){
        Time.timeScale = 1;

        endMenuUI.SetActive(false);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Application.LoadLevel(SceneManager.GetActiveScene().name);
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //activate pause menu
    public void activateMenu(){
        //Debug.Log("Activate Menu");
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
    }

    //deactivate pause menu
    public void deactivateMenu(){
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
        isPaused = false;

    }

    public void activateEndMenu(){
        pauseMenuUI.SetActive(false);
        panelUI.SetActive(false);
        endMenuUI.SetActive(true);
        Time.timeScale = 0;
    }
}
