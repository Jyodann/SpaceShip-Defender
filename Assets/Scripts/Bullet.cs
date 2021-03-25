using UnityEngine;

public class Bullet : MonoBehaviour
{
    private FireBullets fireBullets;
    private Rigidbody2D rb2d;

    private void Start()
    {
        fireBullets = Player.Instance.fireBullets;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity =
            transform.TransformDirection(Vector3.up * fireBullets.bulletSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) return;
        var damageComponent = other.gameObject.GetComponent<IDamageable>();

        if (damageComponent == null) return;
        damageComponent.TakeDamage(other, fireBullets.damageDealt);
        Destroy(gameObject);
    }
}