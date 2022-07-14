using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private FireBullets fireBullets;
    private Rigidbody2D rb2d;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) return;
        var damageComponent = other.gameObject.GetComponent<IDamageable>();

        if (damageComponent == null) return;
        damageComponent.TakeDamage(other, fireBullets.damageDealt);
    }

    public void OnObjectSpawn(Vector2 position, Quaternion rotation)
    {
        var transform1 = transform;
        transform1.position = position;
        transform1.rotation = rotation;
        rb2d.velocity = transform.TransformDirection(Vector3.up * fireBullets.bulletSpeed);
    }

    public void InitialSetUp()
    {
        fireBullets = Player.Instance.fireBullets;
        rb2d = GetComponent<Rigidbody2D>();
    }
}