using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{
    private FireBullets fireBullets;
    private Rigidbody2D rb2d;

    private void Awake()
    {
       
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) return;
        var damageComponent = other.gameObject.GetComponent<IDamageable>();

        if (damageComponent == null) return;
        damageComponent.TakeDamage(other, fireBullets.damageDealt);
        //Destroy(gameObject);
    }

    public void OnObjectSpawn()
    {
        rb2d.velocity = transform.TransformDirection(Vector3.up * fireBullets.bulletSpeed);
    }

    public void InitialSetUp()
    {
        print("Initial Setup Complete!");
        fireBullets = Player.Instance.fireBullets;
        rb2d = GetComponent<Rigidbody2D>();
    }
}