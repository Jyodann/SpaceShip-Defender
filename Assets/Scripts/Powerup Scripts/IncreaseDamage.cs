using System.Collections;
using UnityEngine;

namespace Powerup_Scripts
{
    public class IncreaseDamage : Powerup
    {
        //used to track the ship's damage BEFORE the powerup changes it:
        private int initialDamageDealt;

        protected override void TriggerPowerUpEffect()
        {
            //Temporarily stores the initialDamage from the FireBullets component:
            initialDamageDealt = Player.Instance.fireBullets.damageDealt;

            Player.Instance.fireBullets.damageDealt += 2;
            base.TriggerPowerUpEffect();
        }

        public override void ResetPowerupEffect()
        {
            Player.Instance.fireBullets.damageDealt = initialDamageDealt;
        }
    }
}