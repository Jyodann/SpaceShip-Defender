using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidScript : MonoBehaviour
{
    [SerializeField] private Sprite[] asteroidSprites;
    [SerializeField] private GameObject asteroidToSpawn;

    public enum AsteroidSize { Large, Medium, Small };

    public AsteroidSize asteroidSize;
    public float rotateSpeed = 10f;
    public Vector2 originalVelocity;

    // Start is called before the first frame update
    private void Start()
    {
        switch (asteroidSize)
        {
            case AsteroidSize.Large:
                transform.localScale = new Vector3(3, 3, 3);
                break;

            case AsteroidSize.Medium:
                transform.localScale = new Vector3(2, 2, 2);
                break;

            case AsteroidSize.Small:
                transform.localScale = new Vector3(1, 1, 1);
                break;

            default:
                break;
        }
        GetComponent<SpriteRenderer>().sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];
        GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-30, 30), Random.Range(-30, 30));

        originalVelocity = GetComponent<Rigidbody2D>().velocity;
    }

    private void Update()
    {
        transform.Rotate(0, 0, 10 * Time.deltaTime * rotateSpeed);
    }

    public void SpawnChildAsteroids()
    {
        switch (asteroidSize)
        {
            case AsteroidSize.Large:
                var mediumAsteroidsToSpawn = Random.Range(1, 4);
                for (int i = 0; i < mediumAsteroidsToSpawn; i++)
                {
                    Instantiate(asteroidToSpawn, transform.position, Quaternion.identity);
                }
                break;

            case AsteroidSize.Medium:
                var smallAsteroidsToSpawn = Random.Range(1, 6);
                for (int i = 0; i < smallAsteroidsToSpawn; i++)
                {
                    Instantiate(asteroidToSpawn, transform.position, Quaternion.identity);
                }
                break;
        }

        Destroy(gameObject);
    }
}