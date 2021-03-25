using System.Collections;
using UnityEngine;

namespace EnemyScripts
{
    public class UFO : EnemyBase
    {
        //Changes how fast aliens spawn
        [SerializeField] private float alienSpawnRate = 8f;

        //Changes the fastest rate aliens should spawn at MaxDifficulty:
        [SerializeField] private float maxDifficultySpawnRate = 6f;

        //UFO De-spawn timer (Changes when UFO automatically disappears):
        [SerializeField] private float ufoDespawnRate = 40f;

        //Uses an ailen prefab for spawnning aliens:
        [SerializeField] private GameObject alienObject;

        public override void Start()
        {
            base.Start();
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
                while (isFrozen) yield return null;
                //Spawns alien in current UFO locaiton:
                Instantiate(alienObject, transform.position, Quaternion.identity);
                yield return new WaitForSecondsRealtime(alienSpawnRate);
            }
        }

        protected override void EnemyDeath()
        {
            GameManager.Instance.PlayExplosionAnimation(currentCollision.transform,
                OnDeathAnimation.ExplosionTypes.UfoExplosion);
            base.EnemyDeath();
        }
    }
}