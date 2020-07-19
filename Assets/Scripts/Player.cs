using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isInvincible = false;
    private bool isFlickering = false;
    public float flickerRate = 0.3f;
    public float invincibilityLength = 2f;

    public void TakeDamage(int damageTaken)
    {
        if (!isInvincible)
        {
            StartCoroutine(DisableInvincibility(invincibilityLength));
            StartCoroutine(Flicker(flickerRate));
            isInvincible = true;
            GameManager.instance.TakeDamage(damageTaken);
            GameManager.instance.PlayExplosionAnimation(transform, OnDeathAnimation.ExplosionTypes.SmallExplosion);
        }
    }

    private IEnumerator DisableInvincibility(float invincibilityLength)
    {
        yield return new WaitForSecondsRealtime(invincibilityLength);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        isInvincible = false;
        StopAllCoroutines();
    }

    private IEnumerator Flicker(float flickerRate)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(flickerRate);
            isFlickering = !isFlickering;
            gameObject.GetComponent<SpriteRenderer>().color = isFlickering ? new Color(1f, 1f, 1f, 0.2f) : new Color(1f, 1f, 1f, 1f);
        }
    }
}