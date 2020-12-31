using System.Collections;
using UnityEngine;

public class UFO : MonoBehaviour
{
    //Changes how fast aliens spawn
    [SerializeField] private float alienSpawnRate = 8f;

    //Changes the fastest rate aliens should spawn at MaxDifficulty:
    [SerializeField] private float maxDifficultySpawnRate = 6f;

    //UFO De-spawn timer (Changes when UFO automatically disappears):
    [SerializeField] private float ufoDespawnRate = 40f;

    //Boolean that is public so that it can be changed by the GameManager during timeFreeze:
    public bool isAlienSpawn = true;

    //Uses an ailen prefab for spawnning aliens:
    [SerializeField] private GameObject alienObject;

    private void Start()
    {
        //Changes alienSpawnRate based on difficulty
        alienSpawnRate -= SpawningManagement.Factor / 10;
        alienSpawnRate = Mathf.Clamp(alienSpawnRate, maxDifficultySpawnRate, alienSpawnRate);
        //Destroys the UFO object if player is unable to kill it in the set amount of time:
        Destroy(gameObject, ufoDespawnRate);
        //Starts an ailen spawnner:
        StartCoroutine(SpawnAliens());
    }

    private IEnumerator SpawnAliens()
    {
        //Coroutine spawns an ailen in the UFO location every few seconds:
        while (true)
        {
            //If game is paused, or timeFreeze powerup is true, this coroutine will not do anything
            while (!isAlienSpawn) yield return null;
            //Spawns alien in current UFO locaiton:
            Instantiate(alienObject, transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(alienSpawnRate);
        }
    }
}