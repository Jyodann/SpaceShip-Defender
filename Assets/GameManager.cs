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

    public Transform[] spawnLocations;
    public Transform playerLocation;

    public Text livesText;
    public Text coinsText;
    public Text scoreText;

    private void Awake()
    {
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
        livesText.text = Lives.ToString();
        scoreText.text = Score.ToString().PadLeft(8, '0');
        coinsText.text = Coins.ToString();
    }

    public void TakeDamage()
    {
        Lives--;
    }

    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;
    }

    public void AddCoins(int coinsToAdd)
    {
        Coins += coinsToAdd;
    }

    private void Start()
    {
    }
}