using System;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var damageComponent = other.gameObject.GetComponent<IDamageable>();

        if (damageComponent == null) return;
        damageComponent.TakeDamage(other);
        Destroy(gameObject);
    }
    
}
