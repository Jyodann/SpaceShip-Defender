using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    [SerializeField] private Sprite[] asteroidSprites;
    [SerializeField] private GameObject asteroidToSpawn;

    public enum AsteroidSize { Large, Medium, Small };

    public AsteroidSize asteroidSize;
    public float rotateSpeed = 10f;
    private EnemyBehaviour enemyBehaviorScript;

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
        GetComponent<SpriteRenderer>().sprite = asteroidSprites[UnityEngine.Random.Range(0, asteroidSprites.Length)];
        GetComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(-30, 30), UnityEngine.Random.Range(-30, 30));
        enemyBehaviorScript = GetComponent<EnemyBehaviour>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0, 0, 10 * Time.deltaTime * rotateSpeed);
    }

    private void OnDestroy()
    {
        switch (asteroidSize)
        {
            case AsteroidSize.Large:
                //Instantiate 2 Medium:
                SpawnAsteroids(2);
                break;

            case AsteroidSize.Medium:
                SpawnAsteroids(4);
                break;
        }
    }

    private void SpawnAsteroids(int numberToSpawn)
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            Instantiate(asteroidToSpawn, transform.position, Quaternion.identity);
        }
    }
}