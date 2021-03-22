namespace Powerup_Scripts
{
    public class HeartPowerup : Powerup
    {
        protected override void TriggerPowerUpEffect()
        {
            base.TriggerPowerUpEffect();
        
            GameManager.instance.AddLives(1);
        
            StartCoroutine(DisableThenDestroy(2f));
        }
    }
}
