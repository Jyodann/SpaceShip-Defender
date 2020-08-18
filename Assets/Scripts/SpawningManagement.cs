using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningManagement : MonoBehaviour
{
    [SerializeField] private float asteroidSpawnRate = 1.5f;
    [SerializeField] private float alienSpawnRate = 10f;
    [SerializeField] private float ufoSpawnRate = 60f;

    [SerializeField] private float maxDifficultyAsteroidSpawnRate = 0.85f;
    [SerializeField] private float maxDifficultyAlienSpawnRate = 6f;
    [SerializeField] private float maxDifficultyUFOSpawnRate = 30f;

    [SerializeField] private Transform[] ufoSpawnLocations;
    [SerializeField] private Transform playerLocation;
    [SerializeField] private GameObject[] asteroidObjects;
    [SerializeField] private GameObject[] alienObjects;
    [SerializeField] private GameObject[] ufoObjects;

    private float currentFactor = -1f;
    private List<Coroutine> coroutineList = new List<Coroutine>();

    public static float Factor { get; set; }

    /*
     * OnStart, this Manager gets the player's current location and stores it in memory.
     * It also starts the CoRoutine CheckDifficulty so the spawnning will begin
    */

    private void Start()
    {
        //Finds the player and gets the currentTransform:
        playerLocation = FindObjectOfType<Player>().transform;
        StartCoroutine(CheckDifficulty(false));
    }

    /// <summary>
    /// CheckDifficulty is a CoroutineCall that periodically checks the score to scale the difficulty
    /// based on how far the player is in the game. This makes the game less boring as the player will
    /// encounter more enemies and lower Spawn rates as the game goes by
    /// </summary>
    /// <param name="forceUpdate">ForceUpdate will force the method call to stop ALL Spawn Coroutines, recalculate difficulty and start spawnning again
    /// It is typically used for development purposes. </param>
    /// <returns></returns>
    private IEnumerator CheckDifficulty(bool forceUpdate)
    {
        while (true)
        {
            //While condition checks if 1. Game is Paused OR 2. Time Freeze Powerup is active.
            //It stops this enumerator from running temporarily.
            //This code is taken from https://answers.unity.com/questions/904429/pause-and-resume-coroutine-1.html
            while (GameManager.instance.isPaused || GameManager.instance.IsTimeFrozen)
            {
                yield return null;
            }

            //Difficulty Factor is Calculated with a pre-determined formula of Score/250:
            int difficultyFactor = GameManager.instance.Score / 250;

            print("Current Difficulty Factor: (x) " + difficultyFactor);
            //actual factor (used to tune game Health, and spawn rates) is calculated with this formula:
            // factor = (1.05 ^ difficultyFactor - 1)
            // Difficulty Scaling concept inspired by Unity's 2D RogueLike LevelGeneration Tutorial:
            // https://learn.unity.com/tutorial/level-generation?uv=5.x&projectId=5c514a00edbc2a0020694718#5c7f8528edbc2a002053b6f6

            Factor = (Mathf.Pow(1.05f, difficultyFactor) - 1);
            print("Current Factor: (y)" + Factor);

            /*Spawnning Logic:
             This will first check if the factor is currently the same. If it is, it will not stop the current spawnRates/change
             difficulty as it is a relatively expensive operation.
            */
            if (Factor != currentFactor || forceUpdate)
            {
                //On first run, the coroutine list is Empty, this code is to prevent a Null-pointer Error.
                if (coroutineList.Count >= 0)
                {
                    //If current SpawnRates are not correct, it stops all previous co-routine calls to refresh it.
                    foreach (var coroutine in coroutineList)
                    {
                        StopCoroutine(coroutine);
                    }
                }

                //stores current factor as the new Factor number for future reference.
                currentFactor = Factor;

                //All spawnRate calculations are done here. The baseSpawnRate is deducted by the factor,
                //Hence as the game progresses, spawnning gets quicker and quicker.
                //The values are clamped to balance the game as you do not want a spawnRate that is too quick:
                var asteroidSpawn = Mathf.Clamp((asteroidSpawnRate - Factor), maxDifficultyAsteroidSpawnRate, asteroidSpawnRate);
                var alienSpawn = Mathf.Clamp((alienSpawnRate - Factor), maxDifficultyAlienSpawnRate, alienSpawnRate);
                var ufoSpawn = Mathf.Clamp(ufoSpawnRate - Factor, maxDifficultyUFOSpawnRate, ufoSpawnRate);

                print($"Current SpawnRates: {alienSpawn} (Alien) {asteroidSpawn} (Asteroid) {ufoSpawn} (UFO)");

                //Starts spawnning Asteroids, and adds it to the coroutine List:
                //** Spawner Coroutine mentioned later.
                coroutineList.Add(StartCoroutine(Spawner(asteroidObjects, playerLocation.transform, asteroidSpawn, 25f, 50f)));

                if (GameManager.instance.Score >= 500)
                {
                    //Starts spawnning Aliens after score is more than 500, and adds it to the coroutine List:
                    coroutineList.Add(StartCoroutine(Spawner(alienObjects, playerLocation.transform, alienSpawn, 40f, 80f)));
                }
                if (GameManager.instance.Score >= 1500)
                {
                    //Starts spawnning UFOs after score is more than 1500, and adds it to the coroutine List:
                    coroutineList.Add(StartCoroutine(Spawner(ufoObjects,
                        ufoSpawnLocations[Random.Range(0, ufoSpawnLocations.Length)], ufoSpawn, 1f, 4f)));
                }
            }
            //Checks Difficulty every 10 seconds
            yield return new WaitForSeconds(10f);
        }
    }

    /// <summary>
    /// Spawner is a Generic Coroutine Call that spawns objects periodically at a location
    /// It randomly decides a location between min and max distance, away from the Relative Location.
    /// </summary>
    /// <param name="objectsToSpawn"> ObjectsToSpawn[] is the Array of all possible spawn states of any given enemy </param>
    /// <param name="relativeTo"> RelativeTo determines what location this object should spawn in </param>
    /// <param name="spawnRate"> SpawnRate determines how many times this object is spawned every second</param>
    /// <param name="minimumDistanceAway">This refers to the minimum distance away from the Relative Transform the object should spawn in</param>
    /// <param name="maximumDistanceAway">This refers to the maximum distance away from the Relative Transform the object should spawn in</param>
    /// <returns></returns>
    private IEnumerator Spawner(GameObject[] objectsToSpawn, Transform relativeTo, float spawnRate, float minimumDistanceAway, float maximumDistanceAway)
    {
        while (true)
        {
            //Pauses method call when Game is paused or TimeFreeze Powerup is active.
            //This code is taken from https://answers.unity.com/questions/904429/pause-and-resume-coroutine-1.html
            while (GameManager.instance.isPaused || GameManager.instance.IsTimeFrozen)
            {
                yield return null;
            }
            //Chooses a randomObject from the objectsToSpawn array to spawn:
            var objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

            //randomly decide a region to spawn in:
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
            //calls method again based on spawnRate
            yield return new WaitForSecondsRealtime(spawnRate);
        }
    }
}