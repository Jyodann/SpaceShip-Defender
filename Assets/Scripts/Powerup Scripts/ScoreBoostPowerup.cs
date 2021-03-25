using System.Collections;
using UnityEngine;

namespace Powerup_Scripts
{
    public class ScoreBoostPowerup : Powerup
    {
        protected override void TriggerPowerUpEffect()
        {
            
            //Tells game manager to add DoubleScore:
            GameManager.Instance.ChangeDoubleScore(true);
            //Starts Coroutine to reset double score after 10 seconds:
            //StartCoroutine(ResetDoubleScore(10f));
            base.TriggerPowerUpEffect();
        }
        
        public override void ResetPowerupEffect()
        {
            GameManager.Instance.ChangeDoubleScore(false);
        }
    }
}