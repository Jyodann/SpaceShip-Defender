using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningManagement : MonoBehaviour
{
    [SerializeField] private float asteroidSpawnRate = 1.5f;
    [SerializeField] private float alienSpawnRate = 10f;
    [SerializeField] private float UFOSpawnRate = 60f;
    [SerializeField] private Transform[] ufoSpawnLocations;
    [SerializeField] private Transform playerLocation;
    [SerializeField] private GameObject[] asteroidObjects;
    [SerializeField] private GameObject[] alienObjects;
    [SerializeField] private GameObject[] ufoObjects;

    private float currentFactor = -1f;
    private List<Coroutine> coroutineList = new List<Coroutine>();
    private bool isPaused = false;

    private void Start()
    {
        playerLocation = FindObjectOfType<Player>().transform;
        StartCoroutine(CheckDifficulty(false));
    }

    private IEnumerator CheckDifficulty(bool forceUpdate)
    {
        while (true)
        {
            int difficultyFactor = GameManager.Instance.Score / 500;

            print("Current Difficulty Factor: (x) " + difficultyFactor);

            var factor = (Mathf.Pow(1.05f, difficultyFactor) - 1);

            if (factor != currentFactor || forceUpdate)
            {
                if (coroutineList.Count >= 0)
                {
                    foreach (var coroutine in coroutineList)
                    {
                        StopCoroutine(coroutine);
                    }
                }

                currentFactor = factor;
                var asteroidSpawn = asteroidSpawnRate - Mathf.Clamp(factor, 0f, 0.75f);
                var alienSpawn = alienSpawnRate - Mathf.Clamp(factor, 0f, 5f);
                var ufoSpawn = UFOSpawnRate - Mathf.Clamp(factor, 0f, 30f);
                print($"Current SpawnRates: {alienSpawn} (Alien) {asteroidSpawn} (Asteroid) {ufoSpawn} (UFO)");

                coroutineList.Add(StartCoroutine(Spawner(asteroidObjects, playerLocation.transform, asteroidSpawn, 10f, 50f)));

                if (difficultyFactor > 0)
                {
                    coroutineList.Add(StartCoroutine(Spawner(alienObjects, playerLocation.transform, alienSpawn, 10f, 50f)));
                }
                if (difficultyFactor >= 6)
                {
                    coroutineList.Add(StartCoroutine(Spawner(ufoObjects,
                        ufoSpawnLocations[Random.Range(0, ufoSpawnLocations.Length)], ufoSpawn, 1f, 4f)));
                }
            }

            yield return new WaitForSeconds(5f);
        }
    }

    private IEnumerator Spawner(GameObject[] objectsToSpawn, Transform relativeTo, float spawnRate, float minimumDistanceAway, float maximumDistanceAway)
    {
        while (true)
        {
            var objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
            if (Random.Range(0, 2) == 1)
            {
                //only spawn in the negative regions:
                Instantiate(objectToSpawn,
                    relativeTo.position +
                    new Vector3(Random.Range(-maximumDistanceAway, -minimumDistanceAway), Random.Range(-maximumDistanceAway, -minimumDistanceAway))
                    , Quaternion.identity);
            }
            else
            {
                //only spawn in the positive regions:
                Instantiate(objectToSpawn,
                    relativeTo.position + new Vector3(Random.Range(minimumDistanceAway, maximumDistanceAway),
                    Random.Range(minimumDistanceAway, maximumDistanceAway)), Quaternion.identity);
            }

            yield return new WaitForSecondsRealtime(spawnRate);
        }
    }

    public void ForceSpawningStop(bool isStopped)
    {
        if (isStopped)
        {
            StopAllCoroutines();
        }
        else
        {
            StartCoroutine(CheckDifficulty(true));
        }
    }
}