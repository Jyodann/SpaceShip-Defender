using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

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
        Instantiate(asteroidObject, spawnLocations[UnityEngine.Random.Range(0, 4)].position, Quaternion.identity);
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