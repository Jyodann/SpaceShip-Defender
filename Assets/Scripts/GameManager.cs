using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Why are there 3 different managers?
    ///
    /// The main idea of having 3 different managers is so that Code is easier to maintain as it is not as long, and hence not as messy
    /// The different manager names are distinct, which also lets the developer know which files to change for a specific function
    ///
    /// Though all the other managers can be together in this one file, a decision was made to separate them and instead introduce
    /// helper functions in the main GameManager to access the other managers more easily if required.
    /// This is part of using the Separation of concerns design principle.
    /// </summary>

    //Makes new instance of gameManager, not initialised as it uses a Singleton pattern similar to the one found in:
    //https://learn.unity.com/tutorial/level-generation?uv=5.x&projectId=5c514a00edbc2a0020694718#5c7f8528edbc2a002053b6f7 (2D RougueLike tutorial)
    public static GameManager instance;

    //Declares a static to store highScore for this instance of the game:
    private static int highScore;

    //private health property used within this Script only:
    private int Lives { get; set; }

    //Public score property that is modified and read by this Script and SpawningManagement, keeps track of lives:
    public int Score { get; set; }

    //Public coins propety that is modified and read by this script and Upgrade Management, keeps track of the current number of coins:
    public int Coins { get; set; }

    //Public DoubleScore property which is read by EnemyBehavior, so as to add the appropriate score:
    public bool DoubleScore { get; set; }

    //isPaused keeps Pause State, read by FireBullets and SpawningManagement
    public bool IsPaused { get; set; }

    //isTimeFrozen is responsible for the TimeFreeze powerup, it stops SpawnManagement from spawnning objects:
    public bool IsTimeFrozen { get; set; }

    //All these fields are various UI elements that are set in the inspector:
    [SerializeField] private Text livesText;

    [SerializeField] private Text coinsText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text scoreBoostText;
    [SerializeField] private Text gameOverText;

    //GameOver meny and PauseMenu are both Canvas objects childed under the UIObjects gameObject:
    public GameObject gameOverMenu;

    public GameObject pauseMenu;

    //Enum of GameState to check if Game is over, Paused or currently playing:
    //Future plans to actually add certain BossStates, therefore this is here instead of just using booleans:
    private enum GameState { InGame, Paused, GameOver }

    //Keeps track of the current session's gameState:
    private GameState currentGameState;

    //Keeps a reference to the deathAnimation player
    private OnDeathAnimation deathAnimationManager;

    //Setting Eumn to allow player to change Control mode
    public enum ControlMode { KeyboardOnly, MixedMouseKeyboard }

    //Static Control Mode Enum for reading in the ShipController:
    public static ControlMode playerControlMode = ControlMode.MixedMouseKeyboard;

    private void Awake()
    {
        playerControlMode = (ControlMode)PlayerPrefs.GetInt("controlMode", 1);
        highScore = PlayerPrefs.GetInt("hiScore", 0);

        //These can be changed for testing purposes:
        Lives = 10;
        Coins = 0;
        Score = 0;

        //Uses a SingleTon pattern for GameManager, code based off: https://learn.unity.com/tutorial/level-generation?uv=5.x&projectId=5c514a00edbc2a0020694718#5c7f8528edbc2a002053b6f7 (2D RougueLike tutorial)

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Finds a DeathAnimationManager by the script component:
        deathAnimationManager = FindObjectOfType<OnDeathAnimation>();
        //Sets current game state to InGame:
        currentGameState = GameState.InGame;

        //Sets current Coins, Lives and Score to the UI counterparts:
        coinsText.text = Coins.ToString();
        livesText.text = Lives.ToString();
        scoreText.text = Score.ToString().PadLeft(8, '0');
    }

    private void Update()
    {
        //Checks currentGameState:
        switch (currentGameState)
        {
            case GameState.InGame:
                //If gameState is inGame: Pressing Escape will pause the game
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //Changes Pause State variable, and changes enum to paused:
                    IsPaused = true;
                    currentGameState = GameState.Paused;
                    //Sets timeScale to 0 to ensure nothing moves:
                    Time.timeScale = 0;
                    //Shows the Pause Meny:
                    pauseMenu.SetActive(true);
                }

                #region CheatCodes

                //These are not meant to be in the game, but are added to facilitate testing :)
                //P = Add 50 Coins
                //K = Add 1000 Coins
                //Q = Add 1000 Score
                //I = Take 1 Damage
                //O = Add 1 Life:
                if (Input.GetKeyDown(KeyCode.P))
                {
                    AddCoins(50);
                }

                if (Input.GetKeyDown(KeyCode.K))
                {
                    AddCoins(1000);
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    AddScore(1000);
                }

                if (Input.GetKeyDown(KeyCode.I))
                {
                    TakeDamage(1);
                }

                if (Input.GetKeyDown(KeyCode.O))
                {
                    TakeDamage(-1);
                }

                #endregion CheatCodes

                break;

            case GameState.Paused:
                //If Game is paused, pressing escape will unpause it:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //Set pause state = false
                    IsPaused = false;
                    //Sets currentGameState to be Ingame
                    currentGameState = GameState.InGame;
                    //Disables Pause menu:
                    pauseMenu.SetActive(false);
                    //Sets timeScale back to normal:
                    Time.timeScale = 1;
                }
                //Press R to allow restart during pause:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    //Sets currentGame state to be inGame
                    currentGameState = GameState.InGame;
                    //Resets Time scale
                    Time.timeScale = 1;
                    //Reloads Scene:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                //Press E to allow quitting during pause:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //Sets gameState to be inGame
                    currentGameState = GameState.InGame;
                    //Sets timeScale to be 1
                    Time.timeScale = 1;
                    //Loads Mainmenu:
                    SceneManager.LoadScene(0);
                }
                break;

            case GameState.GameOver:
                //If gameOver, checks to see if the player Preses R, which is to restart, or ESC to go back to main menu:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    //Sets currentGame state to be inGame
                    currentGameState = GameState.InGame;
                    //Resets Time scale
                    Time.timeScale = 1;
                    //Reloads Scene:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    //Sets gameState to be inGame
                    currentGameState = GameState.InGame;
                    //Sets timeScale to be 1
                    Time.timeScale = 1;
                    //Loads Mainmenu:
                    SceneManager.LoadScene(0);
                }
                break;
        }
    }

    //Helper method to deal damage, takes in one parameter damageValue:
    public void TakeDamage(int damageValue)
    {
        //Deducts current lives by damage taken:
        Lives -= damageValue;

        //Sets livesText to be current amount of Lives;
        livesText.text = Lives.ToString();
        //Checks if lives is less than 0 or 0:
        if (Lives <= 0)
        {
            TriggerLoseCondition();
        }
    }

    //Trigger lose condition is called when the player reaches 0 lives:
    private void TriggerLoseCondition()
    {
        //If current score is higher than highscore, set highScore to be score:
        if (Score > highScore)
        {
            highScore = Score;
            PlayerPrefs.SetInt("hiScore", highScore);
        }
        //Set current game state to gameOver:
        currentGameState = GameState.GameOver;
        //Pauses the game:
        IsPaused = true;
        //Shows the gameOverMenu:
        gameOverMenu.SetActive(true);
        //sets the GameOverMenu text to show Score, and Hi score.
        gameOverText.text = $"Game Over\nScore: {Score.ToString().PadLeft(8, '0')}\nHi-Score: {highScore.ToString().PadLeft(8, '0')}";
        //Sets timeScale to frozen:
        Time.timeScale = 0;
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
        coinsText.text = Coins.ToString();
    }

    public void AddLives(int livesToAdd)
    {
        Lives += livesToAdd;
        //Lives are clamped at 9999 to make sure no UI glitches occur:
        Lives = Mathf.Clamp(Lives, 0, 9999);
        livesText.text = Lives.ToString();
    }

    //Change Time Freeze is used by PowerUpScript to manage timeFreeze powerup:
    public void ChangeTimeFreeze(bool isEnabled)
    {
        IsTimeFrozen = isEnabled;
    }

    //Double Score is used by PowerupScript to manage DoubelScore powerup:
    public void ChangeDoubleScore(bool isEnabled)
    {
        DoubleScore = isEnabled;
        if (DoubleScore)
        {
            //Shows UI for x2 next to score if Double score is enabled:
            scoreBoostText.text = "x2";
        }
        else
        {
            //Disables UI for x2 next to score if Double score is enabled:
            scoreBoostText.text = "";
        }
    }

    //Helper Method to play appropriate explosion in Explosion location:
    public void PlayExplosionAnimation(Transform explosionLocation, OnDeathAnimation.ExplosionTypes explosionType)
    {
        deathAnimationManager.MakeExplosion(explosionLocation, explosionType);
    }
}