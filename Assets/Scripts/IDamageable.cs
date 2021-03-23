using UnityEngine;

public interface IDamageable
{
    void TakeDamage(Collider2D collision, int damageDealt);

}
