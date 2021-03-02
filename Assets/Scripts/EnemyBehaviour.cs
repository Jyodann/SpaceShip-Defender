using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    /*Enemy properties that can be set from inspector, like Health,
     *Score and coins to add when enemy dies
     *Damage dealt when enemy collides with enemy
     *maximumPossibleHealth which is a health ceiling that prevents the scaling difficulty from
     *infinitely adding health
    */
    [SerializeField] private float enemyHealth = 100;
    [SerializeField] private int scoreToAdd = 10;
    [SerializeField] private int coinsToAdd = 5;
    [SerializeField] private int damageDealt = 1;
    [SerializeField] private float maximumPossibleHealth;

    //isDead is boolean that temporarily stores a death state so that the deathAnimation does not play more
    //than once
    private bool isDead;

    //FireBullets class is a class file attached to the playerObject, used for easier access
    //to damageDealt
    private FireBullets playerObject;

    private void Start()
    {
        /* enemyHealth is scaled based on difficulty from SpawnManager, game will get more difficult as
         * the player scores a higher score
         */
        enemyHealth *= SpawningManagement.Factor + 1;

        //enemyHealth is then clamped, so it does not scale infinitely
        enemyHealth = Mathf.Clamp(enemyHealth, 0f, maximumPossibleHealth);

        //finds the playerObject's firebullet class
        playerObject = FindObjectOfType<FireBullets>();
    }

    /// <summary>
    ///     OnTriggerStay2D is used to detect bullet collisions over OnTriggerEnter2D
    ///     as bullets may trigger OnTriggerEnter twice if they are not destoryed on time.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Handles bullet collisions with the enemy:
        if (collision.gameObject.CompareTag("Bullet"))
        {
            //Triggers the gameManager to play the smallExplosion effect on the landing, but not killing the
            //asteroid
            GameManager.instance.PlayExplosionAnimation(collision.transform,
                OnDeathAnimation.ExplosionTypes.SmallExplosion);

            //Damages the enemyInstance with currentBullet Damage
            DealDamage(playerObject.damageDealt);

            //Destorys bullet
            Destroy(collision.gameObject);
        }

        //Checks if enemy is dead:
        if (enemyHealth <= 0 && !isDead)
        {
            //sets the isDead boolean to true, so the animation code does not run twice
            isDead = true;

            //Adds coins earned from cenemy to currentCoins in game session
            GameManager.instance.AddCoins(coinsToAdd);

            //Checks if "DoubleScore" powerup is activated
            if (GameManager.instance.isDoubleScore)
                //Adds double the score of enemy the currentScore in Game session
                GameManager.instance.AddScore(scoreToAdd * 2);
            else
                //Adds normal the score of enemy the currentScore in Game session
                GameManager.instance.AddScore(scoreToAdd);

            //Changes behavior based on objectTag:
            switch (gameObject.tag)
            {
                //Asteroids will play a bigExplosion animation
                case "Asteroid":
                    GameManager.instance.PlayExplosionAnimation(collision.transform,
                        OnDeathAnimation.ExplosionTypes.BigExplosion);
                    //trigger's the asteroid's spawnChildAsteroids method to break the asteroid:
                    gameObject.GetComponent<AsteroidScript>().SpawnChildAsteroids();
                    break;
                //UFOS will play a custom explosion, and drop an item:
                case "UFO":
                    GameManager.instance.PlayExplosionAnimation(collision.transform,
                        OnDeathAnimation.ExplosionTypes.UfoExplosion);
                    gameObject.GetComponent<ItemDrop>().DropItem();
                    Destroy(gameObject);
                    break;

                default:

                    gameObject.GetComponent<ItemDrop>().DropItem();
                    Destroy(gameObject);
                    break;
            }
        }

        //Detects if player hits the enemy:
        if (collision.gameObject.CompareTag("Player"))
            //Checks if the player is invincible due to being recently hit:
            if (!collision.GetComponent<Player>().isInvincible)
            {
                //Deducts health from the player:
                collision.GetComponent<Player>().TakeDamage(damageDealt);
                //Destroys the gameObject that the player collides with:
                Destroy(gameObject);
            }
    }

    //Helper method to dealDamage to currentEnemy:
    private void DealDamage(int damageDealt)
    {
        enemyHealth -= damageDealt;
    }
}