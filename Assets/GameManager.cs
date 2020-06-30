using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public static int Lives = 3;
    public static int Score = 0;
    public static int Coins = 0;
    public static int NumberOfEnemies = 0;

    public Transform[] spawnLocations;
    public Transform playerLocation;

    public Text livesText;
    public Text coinsText;
    public Text scoreText;
    public Text upgradeText;

    public GameObject[] asteroidObjects;
    public float spawnRate = 1.5f;

    public static UpgradeClass nextUpgrade;

    public static List<UpgradeClass> upgrades = new List<UpgradeClass>()
    {
        //Upgrade Class Constructor: Name, Cost, CannonCount, FireRate, Damage per shot
        new UpgradeClass("BaseShip", 0, 1, 0.3f, 1),
        new UpgradeClass("+1 Cannon", 100, 2, 0.3f, 1),
        new UpgradeClass("+Fire Rate", 150, 2, 0.2f, 1),
        new UpgradeClass("+Increased Damage", 200, 2, 0.2f, 3),
        new UpgradeClass("+1 Cannon", 250, 3, 0.2f, 3),
        new UpgradeClass("+Fire Rate", 300, 3, 0.1f, 3),
        new UpgradeClass("+Increased Damage", 350, 3, 0.1f, 5)
    };

    private FireBullets playerFireBullets;
    private bool nextUpgradePossible = true;

    private void Awake()
    {
        playerFireBullets = GameObject.FindObjectOfType<FireBullets>();
        upgradeText.text = string.Empty;
        nextUpgrade = upgrades[1];
        InvokeRepeating("SpawnEnemies", 0f, spawnRate);
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnEnemies()
    {
        if (Random.Range(0, 2) == 1)
        {
            //only spawn in the negative regions:
            Instantiate(asteroidObjects[Random.Range(0, asteroidObjects.Length)], playerLocation.position + new Vector3(Random.Range(-50, -5), Random.Range(-50, -5)), Quaternion.identity);
        }
        else
        {
            //only spawn in the positive regions:
            Instantiate(asteroidObjects[Random.Range(0, asteroidObjects.Length)], playerLocation.position + new Vector3(Random.Range(5, 50), Random.Range(5, 50)), Quaternion.identity);
        }
    }

    public void UpdateDifficulty()
    {
        int difficultyFactor = Score / 1000 + 1;
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
        UpdateDifficulty();
    }

    public void AddCoins(int coinsToAdd)
    {
        Coins += coinsToAdd;
        coinsText.text = Coins.ToString();
    }

    private void Start()
    {
        coinsText.text = Coins.ToString();
        livesText.text = Lives.ToString();
        scoreText.text = Score.ToString().PadLeft(8, '0');
    }

    private void Update()
    {
        print(nextUpgrade.UpgradeName);
        if (nextUpgrade.UpgradeCost <= Coins && nextUpgradePossible)
        {
            upgradeText.text = $"{nextUpgrade.UpgradeName} (Press B To Upgrade)";
            if (Input.GetKeyDown(KeyCode.B))
            {
                playerFireBullets.DamageDealt = nextUpgrade.DamageCount;
                playerFireBullets.CannonCount = nextUpgrade.CannonCount;
                playerFireBullets.fireRate = nextUpgrade.FireRate;
                Coins -= nextUpgrade.UpgradeCost;
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
    }
}