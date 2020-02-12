using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score;

    public bool paused;

    // instance
    public static GameManager instance;

    void Awake ()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update ()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            TogglePauseGame();
        }
    }

    // pauses or unpauses the game
    public void TogglePauseGame ()
    {
        paused = !paused;

        if(paused)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;

        GameUI.instance.TogglePauseScreen(paused);
    }

    // adds to the player's score
    public void AddScore (int scoreToGive)
    {
        score += scoreToGive;
        GameUI.instance.UpdateScoreText();
    }

    // called when the player enters a goal
    public void LevelEnd ()
    {
        // is this the last level?
        if(SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex + 1)
        {
            WinGame();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    // called when the player wins the game
    public void WinGame ()
    {
        GameUI.instance.SetEndScreen(true);
        Time.timeScale = 0.0f;
    }

    // called when the player hits an enemy
    public void GameOver ()
    {
        GameUI.instance.SetEndScreen(false);
        Time.timeScale = 0.0f;
    }
}