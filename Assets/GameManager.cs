using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    private int Lives = 10;
    private int Score = 0;
    private int Coins = 0;
    private int NumberOfEnemies = 0;
    public bool doubleScore { get; set; }

    public Transform[] spawnLocations;
    public Transform playerLocation;

    public Text livesText;
    public Text coinsText;
    public Text scoreText;
    public Text upgradeText;

    public GameObject[] asteroidObjects;
    public GameObject[] alienObjects;

    public float asteroidSpawnRate = 1.5f;
    public float alienSpawnRate = 10f;

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
        doubleScore = false;
        coinsText.text = Coins.ToString();
        livesText.text = Lives.ToString();
        scoreText.text = Score.ToString().PadLeft(8, '0');

        InvokeRepeating("CheckDifficulty", 0f, 5f);
    }

    private void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.O))
        {
            CheckDifficulty();
        }
    }

    private void SpawnAsteroids()
    {
        Spawner(asteroidObjects, playerLocation.transform);
    }

    public void CheckDifficulty()
    {
        int difficultyFactor = Score / 500;

        print("Current Difficulty Factor: (x) " + difficultyFactor);

        float factor = (Mathf.Pow(1.05f, difficultyFactor) - 1);

        print("Current Factor: (y)" + factor);

        CancelInvoke("SpawnAsteroids");
        CancelInvoke("SpawnAliens");

        var asteroidSpawn = asteroidSpawnRate - Mathf.Clamp(factor, 0f, 0.75f);
        var alienSpawn = alienSpawnRate - Mathf.Clamp(factor, 0f, 5f);

        print($"Current SpawnRates: {alienSpawn} (Alien) {asteroidSpawn} (Asteroid)");

        InvokeRepeating("SpawnAsteroids", 0f, asteroidSpawn);

        if (factor > 0)
        {
            InvokeRepeating("SpawnAliens", 0f, alienSpawn);
        }
    }

    private void SpawnAliens()
    {
        Spawner(alienObjects, spawnLocations[Random.Range(0, spawnLocations.Length)]);
    }

    private void Spawner(GameObject[] objectsToSpawn, Transform relativeTo)
    {
        if (Random.Range(0, 2) == 1)
        {
            //only spawn in the negative regions:
            Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Length)],
                relativeTo.position + new Vector3(Random.Range(-50, -5), Random.Range(-50, -5)), Quaternion.identity);
        }
        else
        {
            //only spawn in the positive regions:
            Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Length)],
                relativeTo.position + new Vector3(Random.Range(5, 50), Random.Range(5, 50)), Quaternion.identity);
        }
    }

    public void TakeDamage()
    {
        Lives--;
        livesText.text = Lives.ToString();
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
    }
}