using System.Collections;
using UnityEngine;

public class IncreaseDamage : Powerup
{
    //used to track the ship's damage BEFORE the powerup changes it:
    private int initialDamageDealt;

    protected override void TriggerPowerUpEffect()
    {
        base.TriggerPowerUpEffect();

        //Temporarily stores the initialDamage from the FireBullets component:
        initialDamageDealt = Player.Instance.fireBullets.damageDealt;
        //Changes the damage dealt by bullets to be 2 more than current value:
        Player.Instance.fireBullets.damageDealt += 2;
        //Starts Coroutine to reset damage after 10 seconds
        StartCoroutine(ResetPlayerDamage(10f));
    }

    private IEnumerator ResetPlayerDamage(float resetDelay)
    {
        StartCoroutine(DisableThenDestroy(resetDelay));
        yield return new WaitForSecondsRealtime(resetDelay);
        Player.Instance.fireBullets.damageDealt = initialDamageDealt;
    }
}