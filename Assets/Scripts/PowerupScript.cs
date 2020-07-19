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
    private int initialDamageDealt;

    // Start is called before the first frame update
    private void Start()
    {
        //finds first instance of player GameObject:
        playerObject = GameObject.FindGameObjectWithTag("Player");

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerPowerUpEffect();
        }
    }

    private void TriggerPowerUpEffect()
    {
        switch (powerUp)
        {
            case PowerUps.HeartPowerup:
                GameManager.instance.AddLives(1);
                StartCoroutine(DisableThenDestroy(0f));
                break;

            case PowerUps.IncreaseDamage:
                initialDamageDealt = playerObject.GetComponent<FireBullets>().damageDealt;
                playerObject.GetComponent<FireBullets>().damageDealt += 2;
                StartCoroutine(ResetPlayerDamage(10f));
                break;

            case PowerUps.ScoreBoost:
                GameManager.instance.ChangeDoubleScore(true);
                StartCoroutine(ResetDoubleScore(10f));

                break;

            case PowerUps.TimeFreeze:

                GameManager.instance.ChangeTimeFreeze(true);
                var asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
                var aliens = GameObject.FindGameObjectsWithTag("Alien");
                var ufos = GameObject.FindGameObjectsWithTag("UFO");
                foreach (var asteroid in asteroids)
                {
                    asteroid.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                }

                foreach (var alien in aliens)
                {
                    alien.GetComponent<Alien>().isFrozen = true;
                }

                foreach (var ufo in ufos)
                {
                    ufo.GetComponent<Ufo>().isAlienSpawn = false;
                }
                StartCoroutine(ResetTimeFreeze(5f));

                break;

            case PowerUps.SpeedBoost:
                playerObject.GetComponent<ShipController>().ChangeSpeed(125f);
                StartCoroutine(ResetSpeedBoost(5f));

                break;
        }
    }

    private IEnumerator ResetSpeedBoost(float resetDelay)
    {
        StartCoroutine(DisableThenDestroy(resetDelay));
        yield return new WaitForSecondsRealtime(resetDelay);
        playerObject.GetComponent<ShipController>().ChangeSpeed(75f);
    }

    private IEnumerator ResetPlayerDamage(float resetDelay)
    {
        StartCoroutine(DisableThenDestroy(resetDelay));
        yield return new WaitForSecondsRealtime(resetDelay);
        playerObject.GetComponent<FireBullets>().damageDealt = initialDamageDealt;
    }

    private IEnumerator ResetDoubleScore(float resetDelay)
    {
        StartCoroutine(DisableThenDestroy(resetDelay));
        yield return new WaitForSecondsRealtime(resetDelay);
        GameManager.instance.ChangeDoubleScore(false);
    }

    private IEnumerator ResetTimeFreeze(float resetDelay)
    {
        StartCoroutine(DisableThenDestroy(resetDelay));
        yield return new WaitForSecondsRealtime(resetDelay);
        GameManager.instance.ChangeTimeFreeze(false);
        var asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        var aliens = GameObject.FindGameObjectsWithTag("Alien");
        var ufos = GameObject.FindGameObjectsWithTag("UFO");
        foreach (var asteroid in asteroids)
        {
            asteroid.GetComponent<Rigidbody2D>().velocity = asteroid.GetComponent<AsteroidScript>().originalVelocity;
        }

        foreach (var alien in aliens)
        {
            alien.GetComponent<Alien>().isFrozen = false;
        }

        foreach (var ufo in ufos)
        {
            ufo.GetComponent<Ufo>().isAlienSpawn = true;
        }
    }

    private IEnumerator DisableThenDestroy(float destoryDelay)
    {
        transform.position = new Vector2(-200, -100);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        yield return new WaitForSecondsRealtime(destoryDelay);
        Destroy(gameObject);
    }
}