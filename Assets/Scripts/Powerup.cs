using System.Collections;
using UnityEngine;

//Base class to making a powerup, these two attributes ensure that a RigidBody, and SpriteRender is present as these
// are required for this class to work:
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Powerup : MonoBehaviour
{
    public int ItemDropWeight = 100;
    [SerializeField] private AudioClip pickUpPowerUpSound;

    //RigidBody2D reference:
    private Rigidbody2D rb;

    // Start is called before the first frame update
    private void Start()
    {
        //Caches object's rigidBody:
        rb = GetComponent<Rigidbody2D>();
        //Decides a random direction the powerup floats to:
        rb.velocity = new Vector2(Random.Range(-20f, 20f), Random.Range(-20f, 20f));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Detects that a player has collected the powerup:
        if (collision.CompareTag("Player")) TriggerPowerUpEffect();
    }

    protected virtual void TriggerPowerUpEffect()
    {
        AudioManager.Instance.PlaySound(AudioManager.AudioName.SFX_Powerup);
        Destroy(gameObject);
    }

    public virtual void ResetPowerupEffect() { }


    //Disable then destroy takes in one parameter, which is how long until the powerup is destoryed:
    //Uses Coroutine pattern because it needs to have a RealTime scale instead of a gameTime scale:
    //Needs to delay destruction of object so that it has an opportunity to reset it's effect before it destroys itself
    protected IEnumerator DisableThenDestroy(float destoryDelay)
    {
        //Sets the powerup position to be somewhere impossible for the player to reach:
        transform.position = new Vector2(-200, -100);
        //Freezes the position of the powerup
        rb.velocity = new Vector2(0, 0);
        //Waits for time to end before it destorys the object:
        yield return new WaitForSecondsRealtime(destoryDelay);

        Destroy(gameObject);
    }
}