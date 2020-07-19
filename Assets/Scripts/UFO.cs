using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    public float alienSpawnRate = 4f;
    public bool isAlienSpawn = true;
    [SerializeField] private GameObject alienObject;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(SpawnAliens());
    }

    private IEnumerator SpawnAliens()
    {
        while (true)
        {
            while (!isAlienSpawn)
            {
                yield return null;
            }
            Instantiate(alienObject, transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(alienSpawnRate);
        }
    }

    public void SetSpawnRate(float spawnRate)
    {
        alienSpawnRate = spawnRate;
    }
}