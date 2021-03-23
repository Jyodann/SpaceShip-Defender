using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IFreezable, IDamageable
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
    [SerializeField] private int enemyPercentageChanceDrop = 50;

    protected Collider2D currentCollision;

    //isDead is boolean that temporarily stores a death state so that the deathAnimation does not play more
    //than once
    //private bool isDead;

    //FireBullets class is a class file attached to the playerObject, used for easier access
    //to damageDealt

    private void Awake()
    {
        transform.parent = GameManager.instance.EnemyParent;
    }

    public virtual void Start()
    {
        /* enemyHealth is scaled based on difficulty from SpawnManager, game will get more difficult as
         * the player scores a higher score
         */
        enemyHealth *= SpawningManagement.Factor + 1;

        //enemyHealth is then clamped, so it does not scale infinitely
        enemyHealth = Mathf.Clamp(enemyHealth, 0f, maximumPossibleHealth);
    }

    /// <summary>
    ///     OnTriggerStay2D is used to detect bullet collisions over OnTriggerEnter2D
    ///     as bullets may trigger OnTriggerEnter twice if they are not destoryed on time.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        collision.gameObject.GetComponent<IDamageable>().TakeDamage(collision, damageDealt);
        Destroy(gameObject);
    }

    //Helper method to dealDamage to currentEnemy:
    private void DealDamage(float damageDealt)
    {
        enemyHealth -= damageDealt;
    }

    protected virtual void EnemyDeath()
    {
        ItemDrop.Instance.DropItem(transform, enemyPercentageChanceDrop);
        Destroy(gameObject);
    }

    public abstract void OnFreeze();
    public abstract void Unfreeze();


    public void TakeDamage(Collider2D collision, int dmg)
    {
        currentCollision = collision;
        GameManager.instance.PlayExplosionAnimation(collision.transform,
            OnDeathAnimation.ExplosionTypes.SmallExplosion);
        //Triggers the gameManager to play the smallExplosion effect on the landing, but not killing the
        //asteroid
            
        //Damages the enemyInstance with currentBullet Damage
           
        DealDamage(dmg);
        
        print(Player.Instance.fireBullets.damageDealt);

        if (!(enemyHealth <= 0)) return;
        
        //sets the isDead boolean to true, so the animation code does not run twice
            
        //Adds coins earned from enemy to currentCoins in game session
        GameManager.instance.AddCoins(coinsToAdd);

        //Checks if "DoubleScore" powerup is activated
        if (GameManager.instance.isDoubleScore)
            //Adds double the score of enemy the currentScore in Game session
            GameManager.instance.AddScore(scoreToAdd * 2);
        else
            //Adds normal the score of enemy the currentScore in Game session
            GameManager.instance.AddScore(scoreToAdd);
            
        EnemyDeath();
    }
}