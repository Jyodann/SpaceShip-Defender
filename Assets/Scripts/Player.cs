using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isInvincible = false;
    private bool isFlickering = false;
    public float flickerRate = 0.3f;
    public float invincibilityLength = 2f;

    public void Invincibility()
    {
        if (!isInvincible)
        {
            isInvincible = true;
            Invoke("DisableInvincibility", invincibilityLength);
            GameManager.Instance.TakeDamage();
            InvokeRepeating("Flicker", 0f, flickerRate);
        }
    }

    private void DisableInvincibility()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        CancelInvoke();
        isInvincible = false;
    }

    private void Flicker()
    {
        isFlickering = !isFlickering;

        if (isFlickering)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
    }
}