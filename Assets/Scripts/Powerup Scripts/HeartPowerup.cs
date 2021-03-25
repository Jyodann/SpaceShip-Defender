namespace Powerup_Scripts
{
    public class HeartPowerup : Powerup
    {
        protected override void TriggerPowerUpEffect()
        {
            GameManager.Instance.AddLives(1);
            base.TriggerPowerUpEffect();
        }

       
    }
}