using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Powerup_Scripts
{
    public class TimeFreeze : Powerup
    {
        protected override void TriggerPowerUpEffect()
        {
            base.TriggerPowerUpEffect();
            
            //Tells game manager that time is Frozen:
            GameManager.instance.ChangeTimeFreeze(true);
            //Gets all the game objects on screen:
            var asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
            var aliens = GameObject.FindGameObjectsWithTag("Alien");
            var ufos = GameObject.FindGameObjectsWithTag("UFO");

            //Handles asteroids by changing their velocity to 0:
            foreach (var asteroid in asteroids) asteroid.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            //Handles every ailen object by setting "isFrozen" to be true:
            foreach (var alien in aliens) alien.GetComponent<Alien>().isFrozen = true;
            //Handles UFO objects by setting alien Spawning to be false:
            foreach (var ufo in ufos) ufo.GetComponent<UFO>().isAlienSpawn = false;

            //Starts Coroutine to reset time freeze after 5 seconds:
            StartCoroutine(ResetTimeFreeze(5f));
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
                asteroid.GetComponent<Rigidbody2D>().velocity = asteroid.GetComponent<AsteroidScript>().originalVelocity;
            //Resets alien isFrozen to false so they start moving again:s
            foreach (var alien in aliens) alien.GetComponent<Alien>().isFrozen = false;
            //Allows UFOs to start spawning aliens again:
            foreach (var ufo in ufos) ufo.GetComponent<UFO>().isAlienSpawn = true;
        }
    }

}
