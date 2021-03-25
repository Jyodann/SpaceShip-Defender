using System.Collections;
using UnityEngine;

//Base class to making a powerup, these two attributes ensure that a RigidBody, and SpriteRender is present as these
// are required for this class to work:
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Powerup : MonoBehaviour
{
    [SerializeField] private float PowerupTime = 5f;
    public int ItemDropWeight = 100;
    
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
        PowerupDurationManager.Instance.AddTiming(gameObject.name, PowerupTime);
        Destroy(gameObject);
    }

    public virtual void ResetPowerupEffect() { }
    
}