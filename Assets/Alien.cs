using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private Player playerCharacter;
    public float speed = 10f;

    // Start is called before the first frame update
    private void Start()
    {
        playerCharacter = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerCharacter.transform.position, speed * Time.deltaTime);
    }
}