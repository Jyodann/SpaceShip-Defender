using UnityEngine;

namespace Powerup_Scripts
{
    public class TimeFreeze : Powerup
    {
        protected override void TriggerPowerUpEffect()
        {
            //Tells game manager that time is Frozen:
            GameManager.Instance.IsTimeFrozen = true;

            foreach (Transform enemy in GameManager.Instance.EnemyParent)
                enemy.GetComponent<IFreezable>()?.IsFrozen(true);
            base.TriggerPowerUpEffect();
        }

        public override void ResetPowerupEffect()
        {
            foreach (Transform enemy in GameManager.Instance.EnemyParent)
                enemy.GetComponent<IFreezable>()?.IsFrozen(false);
        }
    }
}