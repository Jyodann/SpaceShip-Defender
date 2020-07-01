using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PowerupScript : MonoBehaviour
{
    public enum PowerUps { HeartPowerup, IncreaseDamage, ScoreBoost, TimeFreeze, SpeedBoost };

    public PowerUps powerUp;
    private GameObject playerObject;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D collider;
    private int initialDamageDealt;

    // Start is called before the first frame update
    private void Start()
    {
        //finds first instance of player GameObject:
        playerObject = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();

        //Decides a random direction the powerup floats to:
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(10, 10);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-10, -10);
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("Player collided");
            TriggerPowerUpEffect();
        }
    }

    private void TriggerPowerUpEffect()
    {
        switch (powerUp)
        {
            case PowerUps.HeartPowerup:
                GameManager.Instance.AddLives(1);
                Destroy(gameObject);
                break;

            case PowerUps.IncreaseDamage:
                initialDamageDealt = playerObject.GetComponent<FireBullets>().DamageDealt;
                playerObject.GetComponent<FireBullets>().DamageDealt += 2;
                Invoke("ResetPlayerDamage", 10f);
                DisableThenDestroy(10f);

                break;

            case PowerUps.ScoreBoost:
                break;

            case PowerUps.TimeFreeze:
                GameManager.Instance.asteroidSpawnRate = 100000000;
                var asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

                foreach (var asteroid in asteroids)
                {
                    asteroid.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                }
                Invoke("ResetTimeFreeze", 5f);
                DisableThenDestroy(5f);
                break;

            case PowerUps.SpeedBoost:
                break;

            default:
                break;
        }
    }

    private void ResetPlayerDamage()
    {
        playerObject.GetComponent<FireBullets>().DamageDealt = initialDamageDealt;
    }

    private void ResetTimeFreeze()
    {
        var asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach (var asteroid in asteroids)
        {
            asteroid.GetComponent<Rigidbody2D>().velocity = asteroid.GetComponent<AsteroidScript>().originalVelocity;
        }
    }

    private void DisableThenDestroy(float destoryDelay)
    {
        spriteRenderer.enabled = false;
        collider.enabled = false;
        Destroy(gameObject, destoryDelay);
    }
}