using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PowerupScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        //Decides a random direction the powerup floats to:
        if (Random.Range(0, 2) == 1)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(10, 10);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-10, -10);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}