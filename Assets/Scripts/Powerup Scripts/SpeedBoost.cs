using System.Collections;
using UnityEngine;

namespace Powerup_Scripts
{
    public class SpeedBoost : Powerup
    {
        protected override void TriggerPowerUpEffect()
        {
            Player.Instance.shipController.ChangeSpeed(125f);
            base.TriggerPowerUpEffect();
        }

        public override void ResetPowerupEffect()
        {
            Player.Instance.shipController.ChangeSpeed(75f);
        }
    }
}