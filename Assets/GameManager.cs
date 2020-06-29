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

    public GameObject asteroidObject;

    private void Awake()
    {
        InvokeRepeating("SpawnEnemies", 0f, 1f);
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
    }

    private void SpawnEnemies()
    {
        if (Random.Range(0, 2) == 1)
        {
            //only spawn in the negative regions:
            Instantiate(asteroidObject, playerLocation.position + new Vector3(Random.Range(-50, -5), Random.Range(-50, -5)), Quaternion.identity);
        }
        else
        {
            //only spawn in the positive regions:
            Instantiate(asteroidObject, playerLocation.position + new Vector3(Random.Range(5, 50), Random.Range(5, 50)), Quaternion.identity);
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
}