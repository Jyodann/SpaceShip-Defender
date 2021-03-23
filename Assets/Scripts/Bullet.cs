using System;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    private FireBullets fireBullets;
    private void Start()
    {
        fireBullets = Player.Instance.fireBullets;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) return;
        var damageComponent = other.gameObject.GetComponent<IDamageable>();

        if (damageComponent == null) return;
        damageComponent.TakeDamage(other, fireBullets.damageDealt);
        Destroy(gameObject);
    }
    
}
