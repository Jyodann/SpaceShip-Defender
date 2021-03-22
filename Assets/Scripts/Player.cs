using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    //isInvincible is a variable that can be checked by other classes, mainly the enemy class so that the enemy does not damage the player,
    //this is to balance the game, getting hit will ensure you have a small window of time to fight/escape a bad situation
    public bool isInvincible;

    //flickerRate and Invincibility length are fields that can be changed if neccessary to balance the game:
    [SerializeField] private float flickerRate = 0.3f;

    [SerializeField] private float invincibilityLength = 2f;

    //isFlickering is a variable that toggles on and off to simulate the flickering of the sprite
    //this is used as a visual indicator for the player to know that they are invincible
    private bool isFlickering;

    //Sprite renderer is cached so changes can be made to it's alpha to simulate flickering
    private SpriteRenderer spriteRenderer;
    public FireBullets fireBullets;
    public ShipController shipController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        fireBullets = GetComponent<FireBullets>();
        shipController = GetComponent<ShipController>();
        
        print(fireBullets.damageDealt);
    }

    //Enemies use this method to damage the player, it takes in one parameter of how much damage to apply to the player:
    public void TakeDamage(int damageTaken)
    {
        //This if statement will terminate if the player is invincible so that the player does not take damage:
        if (isInvincible) return;
        //StartsCourtine to disable Invincibility after a set amount of time:
        StartCoroutine(DisableInvincibility(invincibilityLength));
        //Starts Flicker Coroutine to start the visual of flickering to the player
        StartCoroutine(Flicker(flickerRate));
        isInvincible = true;
        //Calls takeDamage in the GameManager for it to update the HealthBar HUD
        GameManager.instance.TakeDamage(damageTaken);
        //Plays animation to show damage taken
        GameManager.instance.PlayExplosionAnimation(transform, OnDeathAnimation.ExplosionTypes.SmallExplosion);
    }

    private IEnumerator DisableInvincibility(float invincibilityLength)
    {
        //Waits for a number of seconds before disabling the Invincibility, and setting the sprite to the default colour:
        yield return new WaitForSecondsRealtime(invincibilityLength);

        spriteRenderer.color = Color.white;
        isInvincible = false;
        StopAllCoroutines();
    }

    private IEnumerator Flicker(float flickerRate)
    {
        //Flickers on and off by toggling the sprite alpha based on isFlickering true/false:
        while (true)
        {
            //Returns method call after waiting for flickerRate:
            yield return new WaitForSecondsRealtime(flickerRate);
            //inverts the isFlickering:
            isFlickering = !isFlickering;
            //uses a Ternary operator to set the colour of the sprite to a tranlucent white if isFlickering is true, and reset the colour if false:
            spriteRenderer.color = isFlickering ? new Color(1f, 1f, 1f, 0.2f) : Color.white;
        }
    }
}