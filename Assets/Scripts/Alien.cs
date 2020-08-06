using UnityEngine;

public class Alien : MonoBehaviour
{
    private Player playerCharacter;

    //Sets speed that alien will follow player
    [SerializeField] private float speed = 10f;

    //IsFrozen represents if i) The game is based or ii) The TimeFreeze powerup is active
    public bool isFrozen = false;

    private void Start()
    {
        //Finds player character object and stores it as a global variable/
        playerCharacter = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (!isFrozen)
        {
            //Actively moves towards player according to set speed:
            //Code used from: https://docs.unity3d.com/ScriptReference/Vector2.MoveTowards.html
            transform.position = Vector2.MoveTowards(transform.position, playerCharacter.transform.position, speed * Time.deltaTime);
        }
    }
}