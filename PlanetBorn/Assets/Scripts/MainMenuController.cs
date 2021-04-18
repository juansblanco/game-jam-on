using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public LevelLoader levelLoader;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            levelLoader.LoadGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            levelLoader.QuitGame();
        }
    }
}
