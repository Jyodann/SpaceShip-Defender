using System.Collections;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    //Changes how fast aliens spawn, will be changed by the spawn Manager to scale difficulty:
    public float alienSpawnRate = 4f;

    public bool isAlienSpawn = true;

    //Uses an ailen prefab for spawnning aliens:
    [SerializeField] private GameObject alienObject;

    private void Start()
    {
        if (Random.Range(0, 2) == 0)
        {
            GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
        //Starts an ailen spawnner:
        StartCoroutine(SpawnAliens());
    }

    private IEnumerator SpawnAliens()
    {
        //Coroutine spawns an ailen in the UFO location every few seconds:
        while (true)
        {
            //If game is paused, or timeFreeze powerup is true, this coroutine will not do anything
            while (!isAlienSpawn)
            {
                yield return null;
            }
            //Spawns alien in current UFO locaiton:
            Instantiate(alienObject, transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(alienSpawnRate);
        }
    }
}