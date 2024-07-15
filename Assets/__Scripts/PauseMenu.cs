using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private  TMP_Text scoreText;

    private bool _isPaused;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        _isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isPaused)
            {
                PauseGame();
            }
            else
            {
                ContinueGame();
            }
        }
        
    }

     public void ContinueGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        _isPaused = false;
    }

    public void PauseGame()
    {
        scoreText.text = $"Your score {Main.S.totalScore}";
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        _isPaused = true;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        _isPaused = false;
        SceneManager.LoadScene("_Scene_0");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
