using UnityEngine;

public class Alien : EnemyBase
{
    //Sets speed that alien will follow player
    [SerializeField] private float speed = 20f;

    //Sets the maximumSpeed that mutated alien will be:
    [SerializeField] private float maxSpeedMutatedAlien = 40f;

    //IsFrozen represents if i) The game is based or ii) The TimeFreeze powerup is active
    public bool isFrozen;
    private Player playerCharacter;

    public override void Start()
    {
        base.Start();
        //Randomly decides if an alien should be "mutated":
        if (Random.Range(0, 2) == 0)
        {
            //This changes the colour of the alien to a random one:
            GetComponent<SpriteRenderer>().color =
                new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            //Changes speed of the alien if it is mutated, the speed is absed on the current DifficultyFactor:
            speed *= SpawningManagement.Factor + 1;
            speed = Mathf.Clamp(speed, 0f, maxSpeedMutatedAlien);
        }

        //Finds player character object and stores it as a global variable
        playerCharacter = FindObjectOfType<Player>();
    }

    public override void OnFreeze()
    {
        
    }

    public override void Unfreeze()
    {
        
    }

    protected override void EnemyDeath()
    {
        base.EnemyDeath();
    }

    private void Update()
    {
        if (!isFrozen)
            //Actively moves towards player according to set speed:
            //Code used from: https://docs.unity3d.com/ScriptReference/Vector2.MoveTowards.html
            transform.position = Vector2.MoveTowards(transform.position, playerCharacter.transform.position,
                speed * Time.deltaTime);
    }
}