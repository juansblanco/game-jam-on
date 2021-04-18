using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Windows")]
    public GameObject loseWindow;
    public GameObject winWindow;
    public GameObject pauseWindow;

    public LevelLoader levelLoader;

    private bool isPaused;
    private bool gameOver;

    private void Start()
    {
        isPaused = false;
        gameOver = false;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    HidePauseWindow();
                }
                else
                {
                    ShowPauseWindow();
                }
            }
        }
    }

    public void ShowLoseWindow()
    {
        Time.timeScale = 0;
        gameOver = true;
        loseWindow.SetActive(true);
    }

    public void ShowWinWindow()
    {
        Time.timeScale = 0;
        gameOver = true;
        winWindow.SetActive(true);
    }

    public void ShowPauseWindow()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        pauseWindow.SetActive(true);
    }

    public void HidePauseWindow()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        pauseWindow.SetActive(false);
    }

    public void RetryGame()
    {
        levelLoader.LoadSameScene();
    }

    public void ExitGame()
    {
        levelLoader.LoadMainMenu();
    }
}
