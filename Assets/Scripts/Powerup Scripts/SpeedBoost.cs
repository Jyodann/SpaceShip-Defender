using System.Collections;
using UnityEngine;

namespace Powerup_Scripts
{
    public class SpeedBoost : Powerup
    {
        protected override void TriggerPowerUpEffect()
        {
            base.TriggerPowerUpEffect();
            //Changes the current speed of the player:
            Player.Instance.shipController.ChangeSpeed(125f);
            // Starts Coroutine to resetSpeed boost after 5 seconds:
            StartCoroutine(ResetSpeedBoost(5f));
        }
    
        //All the following Coroutines are similar in that they take in a ResetDelay, and calls the coRoutine to DisableThenDestory
        //waits for delay, then resets the player state to it's previous state before the powerup:
        private IEnumerator ResetSpeedBoost(float resetDelay)
        {
            StartCoroutine(DisableThenDestroy(resetDelay));
            yield return new WaitForSecondsRealtime(resetDelay);
            Player.Instance.shipController.ChangeSpeed(75f);
        }
    }
}
