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
            base.TriggerPowerUpEffect();
        }
        
        public override void ResetPowerupEffect()
        {
            GameManager.Instance.ChangeDoubleScore(false);
        }
    }
}