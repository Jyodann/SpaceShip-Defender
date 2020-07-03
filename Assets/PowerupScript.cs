using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PowerupScript : MonoBehaviour
{
    public enum PowerUps { HeartPowerup, IncreaseDamage, ScoreBoost, TimeFreeze, SpeedBoost, PierceShot };

    public PowerUps powerUp;
    private GameObject playerObject;
    private SpriteRenderer spriteRenderer;
    private int initialDamageDealt;

    // Start is called before the first frame update
    private void Start()
    {
        //finds first instance of player GameObject:
        playerObject = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

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
    private void OnTriggerStay2D(Collider2D collision)
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
                GameManager.Instance.ChangeDoubleScore(true);
                Invoke("ResetDoubleScore", 10f);
                DisableThenDestroy(10f);
                break;

            case PowerUps.TimeFreeze:

                GameManager.Instance.ChangeTimeFreeze(true);
                var asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
                var aliens = GameObject.FindGameObjectsWithTag("Alien");
                foreach (var asteroid in asteroids)
                {
                    asteroid.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                }

                foreach (var alien in aliens)
                {
                    alien.GetComponent<Alien>().SetFreeze(true);
                }
                Invoke("ResetTimeFreeze", 5f);
                DisableThenDestroy(5f);
                break;

            case PowerUps.SpeedBoost:
                playerObject.GetComponent<ShipController>().ChangeSpeed(125f);
                Invoke("ResetSpeedBoost", 10f);
                DisableThenDestroy(10f);
                break;

            case PowerUps.PierceShot:
                break;

            default:
                break;
        }
    }

    private void ResetSpeedBoost()
    {
        playerObject.GetComponent<ShipController>().ChangeSpeed(75f);
    }

    private void ResetPlayerDamage()
    {
        playerObject.GetComponent<FireBullets>().DamageDealt = initialDamageDealt;
    }

    private void ResetDoubleScore()
    {
        GameManager.Instance.ChangeDoubleScore(false);
    }

    private void ResetTimeFreeze()
    {
        print("ResetTime");
        GameManager.Instance.ChangeTimeFreeze(false);
        var asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        var aliens = GameObject.FindGameObjectsWithTag("Alien");
        foreach (var asteroid in asteroids)
        {
            asteroid.GetComponent<Rigidbody2D>().velocity = asteroid.GetComponent<AsteroidScript>().originalVelocity;
        }

        foreach (var alien in aliens)
        {
            alien.GetComponent<Alien>().SetFreeze(false);
        }
    }

    private void DisableThenDestroy(float destoryDelay)
    {
        spriteRenderer.enabled = false;
        Destroy(gameObject, destoryDelay);
    }
}