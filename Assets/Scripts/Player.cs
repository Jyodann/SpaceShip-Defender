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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInvincible)
        {
            switch (collision.tag)
            {
                case "BoundingBox":
                    break;

                case "Bullet":
                    break;

                default:
                    Invincibility();
                    Invoke("DisableInvincibility", invincibilityLength);
                    GameManager.Instance.TakeDamage();
                    break;
            }
        }
    }

    private void Invincibility()
    {
        isInvincible = true;
        InvokeRepeating("Flicker", 0f, flickerRate);
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