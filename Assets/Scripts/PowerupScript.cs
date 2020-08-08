using System.Collections;
using UnityEngine;

//Base class to making a powerup, these two attributes ensure that a RigidBody, and SpriteRender is present as these
// are required for this class to work:
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PowerupScript : MonoBehaviour
{
    //Enum to manage all the different types of powerups:
    private enum PowerUps { HeartPowerup, IncreaseDamage, ScoreBoost, TimeFreeze, SpeedBoost };
    
    //Allows the powerup ability to be selected from the UnityEditor:
    [SerializeField] PowerUps powerUp;
    
    //References the only playerObject present:
    private GameObject playerObject;
    
    //used to track the ship's damage BEFORE the powerup changes it:
    private int initialDamageDealt;
    
    //RigidBody2D reference:
    private Rigidbody2D rb;

    // Start is called before the first frame update
    private void Start()
    {
        //Caches object's rigidBody:
        rb = GetComponent<Rigidbody2D>();
        //finds first instance of player GameObject:
        playerObject = GameObject.FindGameObjectWithTag("Player");

        //Decides a random direction the powerup floats to:
        if (Random.Range(0, 2) == 1)
        {
           rb.velocity = new Vector2(10, 10);
        }
        else
        {
            rb.velocity = new Vector2(-10, -10);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Detects that a player has collected the powerup:
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
                //Heart powerup calls the gameManager to add a life and change the HUD:
                GameManager.instance.AddLives(1);
                //Calls Disable and Destroy:
                StartCoroutine(DisableThenDestroy(0f));
                break;

            case PowerUps.IncreaseDamage:
                //Temporarily stores the initialDamage from the FireBullets component:
                initialDamageDealt = playerObject.GetComponent<FireBullets>().damageDealt;
                //Changes the damage dealt by bullets to be 2 more than current value:
                playerObject.GetComponent<FireBullets>().damageDealt += 2;
                //Starts Coroutine to reset damage after 10 seconds
                StartCoroutine(ResetPlayerDamage(10f));
                break;

            case PowerUps.ScoreBoost:
                //Tells game manager to add DoubleScore:
                GameManager.instance.ChangeDoubleScore(true);
                //Starts Coroutine to reset double score after 10 seconds:
                StartCoroutine(ResetDoubleScore(10f));

                break;

            case PowerUps.TimeFreeze:
                //Tells game manager that time is Frozen:
                GameManager.instance.ChangeTimeFreeze(true);
                //Gets all the game objects on screen:
                var asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
                var aliens = GameObject.FindGameObjectsWithTag("Alien");
                var ufos = GameObject.FindGameObjectsWithTag("UFO");
                
                //Handles asteroids by changing their velocity to 0:
                foreach (var asteroid in asteroids)
                {
                    asteroid.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                }
                //Handles every ailen object by setting "isFrozen" to be true:
                foreach (var alien in aliens)
                {
                    alien.GetComponent<Alien>().isFrozen = true;
                }
                //Handles UFO objects by setting alien Spawning to be false:
                foreach (var ufo in ufos)
                {
                    ufo.GetComponent<Ufo>().isAlienSpawn = false;
                }
                
                //Starts Coroutine to reset time freeze after 5 seconds:
                StartCoroutine(ResetTimeFreeze(5f));

                break;

            case PowerUps.SpeedBoost:
                //Changes the current speed of the player:
                playerObject.GetComponent<ShipController>().ChangeSpeed(125f);
                // Starts Coroutine to resetSpeed boost after 5 seconds:
                StartCoroutine(ResetSpeedBoost(5f));

                break;
        }
    }
    //All the following Coroutines are similar in that they take in a ResetDelay, and calls the coRoutine to DisableThenDestory
    //waits for delay, then resets the player state to it's previous state before the powerup:
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
        
        //Gets all the objects that are currently frozen:
        var asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        var aliens = GameObject.FindGameObjectsWithTag("Alien");
        var ufos = GameObject.FindGameObjectsWithTag("UFO");
        
        //Resets velocity of asteroid:
        foreach (var asteroid in asteroids)
        {
            asteroid.GetComponent<Rigidbody2D>().velocity = asteroid.GetComponent<AsteroidScript>().originalVelocity;
        }
        //Resets alien isFrozen to false so they start moving again:s
        foreach (var alien in aliens)
        {
            alien.GetComponent<Alien>().isFrozen = false;
        }
        //Allows UFOs to start spawning aliens again:
        foreach (var ufo in ufos)
        {
            ufo.GetComponent<Ufo>().isAlienSpawn = true;
        }
    }
    //Disable then destroy takes in one parameter, which is how long until the powerup is destoryed:
    //Uses Coroutine pattern because it needs to have a RealTime scale instead of a gameTime scale:
    //Needs to delay destruction of object so that it has an opportunity to reset it's effect before it destroys itself
    private IEnumerator DisableThenDestroy(float destoryDelay)
    {
        //Sets the powerup position to be somewhere impossible for the player to reach:
        transform.position = new Vector2(-200, -100);
        //Freezes the position of the powerup
        rb.velocity = new Vector2(0, 0);
        //Waits for time to end before it destorys the object:
        yield return new WaitForSecondsRealtime(destoryDelay);
        
        Destroy(gameObject);
        
    }
}