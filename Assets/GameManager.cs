using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public static int HighScore = 0;

    private int Lives = 1000;
    private int Score = 0;
    private int Coins = 0;
    public bool doubleScore { get; set; }

    public Transform[] alienSpawnLocations;
    public Transform[] ufoSpawnLocations;
    public Transform playerLocation;

    public Text livesText;
    public Text coinsText;
    public Text scoreText;
    public Text upgradeText;
    public Text scoreBoostText;
    public Text gameOverText;

    public GameObject inGameMenu;
    public GameObject gameOverMenu;
    public GameObject pauseMenu;

    public GameObject[] asteroidObjects;
    public GameObject[] alienObjects;
    public GameObject[] ufoObjects;

    public float asteroidSpawnRate = 1.5f;
    public float alienSpawnRate = 10f;
    public float UFOSpawnRate = 60f;

    private List<Coroutine> coroutineList = new List<Coroutine>();

    public enum GameState { InGame, Paused, GameOver }

    public GameState currentGameState;

    public static UpgradeClass nextUpgrade;

    public static List<UpgradeClass> upgrades = new List<UpgradeClass>()
    {
        //Upgrade Class Constructor: Name, Cost, CannonCount, FireRate, Damage per shot
        new UpgradeClass("BaseShip", 0, 1, 0.3f, 1),
        new UpgradeClass("+1 Cannon", 100, 2, 0.3f, 1),
        new UpgradeClass("+Fire Rate", 150, 2, 0.2f, 1),
        new UpgradeClass("+Damage", 200, 2, 0.2f, 3),
        new UpgradeClass("+1 Cannon", 250, 3, 0.2f, 3),
        new UpgradeClass("+Fire Rate", 300, 3, 0.1f, 3),
        new UpgradeClass("+Damage", 350, 3, 0.1f, 5),
        new UpgradeClass("+1 Cannon", 400, 4, 0.1f, 5),
        new UpgradeClass("+Fire Rate", 350, 4, 0.05f, 5),
        new UpgradeClass("+1 Cannon", 350, 5, 0.1f, 5)
    };

    private FireBullets playerFireBullets;
    private bool nextUpgradePossible = true;
    private float currentFactor = -1f;

    private void Awake()
    {
        playerFireBullets = GameObject.FindObjectOfType<FireBullets>();
        upgradeText.text = string.Empty;
        nextUpgrade = upgrades[1];

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentGameState = GameState.InGame;
        doubleScore = false;
        coinsText.text = Coins.ToString();
        livesText.text = Lives.ToString();
        scoreText.text = Score.ToString().PadLeft(8, '0');

        StartCoroutine(CheckDifficulty());
    }

    private void Update()
    {
        switch (currentGameState)
        {
            case GameState.InGame:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentGameState = GameState.Paused;
                    Time.timeScale = 0;
                    pauseMenu.SetActive(true);
                }

                if (nextUpgrade.UpgradeCost <= Coins && nextUpgradePossible)
                {
                    upgradeText.text = $"{nextUpgrade.UpgradeName} (Press B To Upgrade)";
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        playerFireBullets.DamageDealt = nextUpgrade.DamageCount;
                        playerFireBullets.CannonCount = nextUpgrade.CannonCount;
                        playerFireBullets.fireRate = nextUpgrade.FireRate;
                        AddCoins(-nextUpgrade.UpgradeCost);
                        if (upgrades.IndexOf(nextUpgrade) + 1 != upgrades.Count)
                        {
                            nextUpgrade = upgrades[upgrades.IndexOf(nextUpgrade) + 1];
                        }
                        else
                        {
                            nextUpgradePossible = false;
                        }

                        upgradeText.text = string.Empty;
                    }
                }

                if (Input.GetKeyDown(KeyCode.P))
                {
                    AddCoins(50);
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    AddScore(100);
                }

                if (Input.GetKeyDown(KeyCode.I))
                {
                    TakeDamage();
                }
                break;

            case GameState.Paused:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentGameState = GameState.InGame;
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1;
                }
                break;

            case GameState.GameOver:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    currentGameState = GameState.InGame;
                    gameOverMenu.SetActive(false);
                    Time.timeScale = 1;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentGameState = GameState.InGame;
                    gameOverMenu.SetActive(false);
                    Time.timeScale = 1;
                    SceneManager.LoadScene(0);
                }
                break;

            default:
                break;
        }
    }

    private IEnumerator CheckDifficulty()
    {
        while (true)
        {
            int difficultyFactor = Score / 500;

            print("Current Difficulty Factor: (x) " + difficultyFactor);

            var factor = (Mathf.Pow(1.05f, difficultyFactor) - 1);

            if (factor != currentFactor)
            {
                if (coroutineList.Count >= 0)
                {
                    foreach (var coroutine in coroutineList)
                    {
                        StopCoroutine(coroutine);
                    }
                }

                currentFactor = factor;
                var asteroidSpawn = asteroidSpawnRate - Mathf.Clamp(factor, 0f, 0.75f);
                var alienSpawn = alienSpawnRate - Mathf.Clamp(factor, 0f, 5f);
                var ufoSpawn = UFOSpawnRate - Mathf.Clamp(factor, 0f, 30f);
                print($"Current SpawnRates: {alienSpawn} (Alien) {asteroidSpawn} (Asteroid) {ufoSpawn} (UFO)");

                coroutineList.Add(StartCoroutine(Spawner(asteroidObjects, playerLocation.transform, asteroidSpawn, 10f, 50f)));

                if (difficultyFactor > 0)
                {
                    coroutineList.Add(StartCoroutine(Spawner(alienObjects, playerLocation.transform, alienSpawn, 10f, 50f)));
                }
                if (difficultyFactor >= 6)
                {
                    coroutineList.Add(StartCoroutine(Spawner(ufoObjects, ufoSpawnLocations[Random.Range(0, ufoSpawnLocations.Length)], ufoSpawn, 1f, 4f)));
                }
            }

            yield return new WaitForSeconds(5f);
        }
    }

    private IEnumerator Spawner(GameObject[] objectsToSpawn, Transform relativeTo, float spawnRate, float minimumDistanceAway, float maximumDistanceAway)
    {
        while (true)
        {
            var objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
            if (Random.Range(0, 2) == 1)
            {
                //only spawn in the negative regions:
                Instantiate(objectToSpawn,
                    relativeTo.position +
                    new Vector3(Random.Range(-maximumDistanceAway, -minimumDistanceAway), Random.Range(-maximumDistanceAway, -minimumDistanceAway))
                    , Quaternion.identity);
            }
            else
            {
                //only spawn in the positive regions:
                Instantiate(objectToSpawn,
                    relativeTo.position + new Vector3(Random.Range(minimumDistanceAway, maximumDistanceAway),
                    Random.Range(minimumDistanceAway, maximumDistanceAway)), Quaternion.identity);
            }

            yield return new WaitForSecondsRealtime(spawnRate);
        }
    }

    public void TakeDamage()
    {
        Lives--;
        livesText.text = Lives.ToString();

        if (Lives <= 0)
        {
            TriggerLoseCondition();
        }
    }

    private void TriggerLoseCondition()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
        }
        currentGameState = GameState.GameOver;
        gameOverMenu.SetActive(true);
        gameOverText.text = $"Game Over\nScore: {Score.ToString().PadLeft(8, '0')}\nHi-Score: {HighScore.ToString().PadLeft(8, '0')}";
        Time.timeScale = 0;
    }

    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
        scoreText.text = Score.ToString().PadLeft(8, '0');
    }

    public void AddCoins(int coinsToAdd)
    {
        Coins += coinsToAdd;
        coinsText.text = Coins.ToString();
    }

    public void AddLives(int livesToAdd)
    {
        Lives += livesToAdd;
        livesText.text = Lives.ToString();
    }

    public void ChangeTimeFreeze(bool isEnabled)
    {
        if (isEnabled)
        {
            CancelInvoke();
        }
        else
        {
            CheckDifficulty();
        }
    }

    public void ChangeDoubleScore(bool isEnabled)
    {
        doubleScore = isEnabled;
        if (doubleScore)
        {
            scoreBoostText.text = "x2";
        }
        else
        {
            scoreBoostText.text = "";
        }
    }
}