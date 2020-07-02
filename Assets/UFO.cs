using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public float alienSpawnRate = 4f;
    [SerializeField] private GameObject alienObject;

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("SpawnAliens", 0f, alienSpawnRate);
    }

    private void SpawnAliens()
    {
        Instantiate(alienObject, transform.position, Quaternion.identity);
    }

    public void SetSpawnRate(float spawnRate)
    {
        alienSpawnRate = spawnRate;
    }
}