using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isInvincible = false;
    private bool isFlickering = false;
    public float flickerRate = 0.3f;
    public float invincibilityLength = 2f;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damageTaken)
    {
        if (isInvincible) return;
        StartCoroutine(DisableInvincibility(invincibilityLength));
        StartCoroutine(Flicker(flickerRate));
        isInvincible = true;
        GameManager.instance.TakeDamage(damageTaken);
        GameManager.instance.PlayExplosionAnimation(transform, OnDeathAnimation.ExplosionTypes.SmallExplosion);
    }

    private IEnumerator DisableInvincibility(float invincibilityLength)
    {
        yield return new WaitForSecondsRealtime(invincibilityLength);
        
        spriteRenderer.color = Color.white;
        isInvincible = false;
        StopAllCoroutines();
    }

    private IEnumerator Flicker(float flickerRate)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(flickerRate);
            isFlickering = !isFlickering;
            spriteRenderer.color = isFlickering ? new Color(1f, 1f, 1f, 0.2f) : Color.white;
        }
    }
}