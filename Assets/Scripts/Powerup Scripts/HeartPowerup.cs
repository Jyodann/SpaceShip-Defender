using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPowerup : Powerup
{

    public override void TriggerPowerUpEffect()
    {
        base.TriggerPowerUpEffect();
        
        GameManager.instance.AddLives(1);
        
        StartCoroutine(DisableThenDestroy(2f));
    }
}
