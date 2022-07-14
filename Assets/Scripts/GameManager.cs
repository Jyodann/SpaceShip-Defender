﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    ///     Why are there 3 different managers?
    ///     The main idea of having 3 different managers is so that Code is easier to maintain as it is not as long, and hence
    ///     not as messy
    ///     The different manager names are distinct, which also lets the developer know which files to change for a specific
    ///     function
    ///     Though all the other managers can be together in this one file, a decision was made to separate them and instead
    ///     introduce
    ///     helper functions in the main GameManager to access the other managers more easily if required.
    ///     This is part of using the Separation of concerns design principle.
    /// </summary>

    //Makes new instance of gameManager, not initialised as it uses a Singleton pattern similar to the one found in:
    //https://learn.unity.com/tutorial/level-generation?uv=5.x&projectId=5c514a00edbc2a0020694718#5c7f8528edbc2a002053b6f7 (2D RougueLike tutorial)

    //Declares a static to store highScore for this instance of the game:
    private static int highScore;

    //All these fields are various UI elements that are set in the inspector:
    [SerializeField] private TextMeshProUGUI livesText;

    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreBoostText;
    [SerializeField] private Text gameOverText;
    [SerializeField] private GameObject joystickUI;
    [SerializeField] private GameObject mainGameUI;

    //GameOver meny and PauseMenu are both Canvas objects childed under the UIObjects gameObject:
    public GameObject gameOverMenu;

    public GameObject pauseMenu;

    public bool IsTimeFrozen;
    //Setting Eumn to allow player to change Control mode


    public bool flipControls;

    //List that stores all the available background music:
    public List<AudioClip> backgroundMusic;

    public Transform EnemyParent;

    //A static bool to track whether audio should be muted:


    //AudioSource List for all the soundtracks:
    //private AudioSource audioSource;

    private readonly string[] backgroundColourList =
        {"#0C2A46", "#4F106F", "#000000", "#500000", "#053D24", "#3C0527", "#221F21", "#41170F"};

    //isTimeFrozen is responsible for the TimeFreeze powerup, it stops SpawnManagement from spawnning objects:
    //public bool IsTimeFrozen { get; set; }
    

    //Keeps track of the current session's gameState:
    private GameState currentGameState;

    //Keeps a reference to the deathAnimation player
    private OnDeathAnimation deathAnimationManager;

    private long startTime;

    //private health property used within this Script only:
    private int Lives { get; set; }

    //Public score property that is modified and read by this Script and SpawningManagement:
    public int Score { get; set; }

    //Public coins propety that is modified and read by this script and Upgrade Management, keeps track of the current number of coins:
    public int Coins { get; set; }

    //Public DoubleScore property which is read by EnemyBehavior, so as to add the appropriate score:
    public bool isDoubleScore { get; set; }

    //isPaused keeps Pause State, read by FireBullets and SpawningManagement
    public bool isPaused { get; set; }

    private void Awake()
    {
        //Get saved hi-Score from PlayerPrefs:
        highScore = PlayerPrefs.GetInt("hiScore", 0);

        //These can be changed for testing purposes:
        Lives = 10;
        Coins = 0;
        Score = 0;

        startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    private void Start()
    {
        Color backgroundColour;
        ColorUtility.TryParseHtmlString(backgroundColourList[Random.Range(0, backgroundColourList.Length)],
            out backgroundColour);
        Camera.main.backgroundColor = backgroundColour;

        //Finds a DeathAnimationManager by the script component:
        deathAnimationManager = FindObjectOfType<OnDeathAnimation>();
        //Sets current game state to InGame:
        currentGameState = GameState.InGame;

        //Sets current Coins, Lives and Score to the UI counterparts:
        coinsText.text = Coins.ToString().PadLeft(4, '0');
        livesText.text = Lives.ToString().PadLeft(2, '0');
        scoreText.text = Score.ToString().PadLeft(8, '0');

        scoreBoostText.text = string.Empty;
        
        if (SettingsHelper.CurrentControlMode == SettingsHelper.ControlMode.MobileInput) joystickUI.SetActive(true);
        print(SettingsHelper.CurrentControlMode);
    }


    private void Update()
    {
        if (SettingsHelper.CurrentControlMode != SettingsHelper.ControlMode.MobileInput) joystickUI.SetActive(false);
        //Mutes/Unmutes the game by pausing the Global Audio listener:
        AudioListener.pause = !SettingsHelper.IsMusicOn;
    }
    

    private void ExitGame()
    {
        //Sets gameState to be inGame
        currentGameState = GameState.InGame;
        //Sets timeScale to be 1
        Time.timeScale = 1;
        //Loads Mainmenu:
        SceneManager.LoadScene(0);
    }


    public void Unpause()
    {
        //Set pause state = false
        isPaused = false;
        //Sets currentGameState to be Ingame
        currentGameState = GameState.InGame;
        //Disables Pause menu:
        pauseMenu.SetActive(false);
        //Sets timeScale back to normal:
        Time.timeScale = 1;
        joystickUI.SetActive(true);
        mainGameUI.SetActive(true);
    }

    public void PauseGame()
    {
        //Changes Pause State variable, and changes enum to paused:
        isPaused = true;
        currentGameState = GameState.Paused;
        //Sets timeScale to 0 to ensure nothing moves:
        Time.timeScale = 0;
        //Shows the Pause Meny:
        pauseMenu.SetActive(true);
        joystickUI.SetActive(false);
        mainGameUI.SetActive(false);
    }

    public void GoToMainMenu()
    {
        //Sets gameState to be inGame
        currentGameState = GameState.InGame;
        //Sets timeScale to be 1
        Time.timeScale = 1;
        //Loads Mainmenu:
        SceneManager.LoadScene(0);
        joystickUI.SetActive(true);
    }

    public void RestartGame()
    {
        //Sets currentGame state to be inGame
        currentGameState = GameState.InGame;
        //Resets Time scale
        Time.timeScale = 1;
        //Reloads Scene:
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        joystickUI.SetActive(false);
    }

    //Helper method to deal damage, takes in one parameter damageValue:
    public void TakeDamage(int damageValue)
    {
        //Deducts current lives by damage taken:
        Lives -= damageValue;

        //Sets livesText to be current amount of Lives;
        livesText.text = Lives.ToString().PadLeft(2, '0');
        //Checks if lives is less than 0 or 0:
        if (Lives <= 0) TriggerLoseCondition();
    }

    //Trigger lose condition is called when the player reaches 0 lives:
    private void TriggerLoseCondition()
    {
        //If current score is higher than highscore, set highScore to be score:
        if (Score > highScore)
        {
            highScore = Score;
            //Set PlayerPrefs to current Hi-Score so that it is saved between game sessions:
            PlayerPrefs.SetInt("hiScore", highScore);
        }

        //Set current game state to gameOver:
        currentGameState = GameState.GameOver;
        //Pauses the game:
        isPaused = true;
        //Shows the gameOverMenu:
        gameOverMenu.SetActive(true);
        //sets the GameOverMenu text to show Score, and Hi score.
        gameOverText.text =
            $"Game Over\nScore: {Score.ToString().PadLeft(8, '0')}\nHi-Score: {highScore.ToString().PadLeft(8, '0')}";
        //Sets timeScale to frozen:
        Time.timeScale = 0;
        joystickUI.SetActive(false);
    }

    //The next 3 'Add' helper methods do about the same thing for Score, Coins and Lives,
    //adds it to the current Variable based on the amount to add in the Parameter and updates the HUD:
    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
        //Although a pretty impossible feat, Score is clamped at 999999999 to ensure no UI Glitches occur:
        Score = Mathf.Clamp(Score, 0, 99999999);
        scoreText.text = Score.ToString().PadLeft(8, '0');
    }

    public void AddCoins(int coinsToAdd)
    {
        Coins += coinsToAdd;
        //Coins are clamped at 9999 to make sure no UI glitches occur:
        Coins = Mathf.Clamp(Coins, 0, 9999);
        coinsText.text = Coins.ToString().PadLeft(4, '0');
    }

    public void AddLives(int livesToAdd)
    {
        Lives += livesToAdd;
        //Lives are clamped at 9999 to make sure no UI glitches occur:
        Lives = Mathf.Clamp(Lives, 0, 9999);
        livesText.text = Lives.ToString().PadLeft(2, '0');
    }

    //Double Score is used by PowerupScript to manage DoubelScore powerup:
    public void ChangeDoubleScore(bool isEnabled)
    {
        isDoubleScore = isEnabled;
        if (isDoubleScore)
            //Shows UI for x2 next to score if Double score is enabled:
            scoreBoostText.text = "x2";
        else
            //Disables UI for x2 next to score if Double score is enabled:
            scoreBoostText.text = "";
    }

    //Helper Method to play appropriate explosion in Explosion location:
    public void PlayExplosionAnimation(Transform explosionLocation, OnDeathAnimation.ExplosionTypes explosionType)
    {
        deathAnimationManager.MakeExplosion(explosionLocation, explosionType);
    }

    //Enum of GameState to check if Game is over, Paused or currently playing:
    //Future plans to actually add certain BossStates, therefore this is here instead of just using booleans:
    private enum GameState
    {
        InGame,
        Paused,
        GameOver
    }
}