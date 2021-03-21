using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoostPowerup : Powerup
{
    public override void TriggerPowerUpEffect()
    {
        base.TriggerPowerUpEffect();
        //Tells game manager to add DoubleScore:
        GameManager.instance.ChangeDoubleScore(true);
        //Starts Coroutine to reset double score after 10 seconds:
        StartCoroutine(ResetDoubleScore(10f));
    }
    
    private IEnumerator ResetDoubleScore(float resetDelay)
    {
        StartCoroutine(DisableThenDestroy(resetDelay));
        yield return new WaitForSecondsRealtime(resetDelay);
        GameManager.instance.ChangeDoubleScore(false);
    }
}
