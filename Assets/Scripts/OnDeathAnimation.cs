using System.Collections.Generic;
using UnityEngine;

public class OnDeathAnimation : MonoBehaviour
{
    /// <summary>
    /// Death Animation manager will be responsible for playing explosions on command.
    /// </summary>

    //These SerialisedFields are for setting the prefabs with a particleSystem in it that has the corresponding animation:
    [SerializeField] private GameObject smallExplosion;

    [SerializeField] private GameObject bigExplosion;
    [SerializeField] private GameObject ufoExplosion;

    [SerializeField] private AudioClip smallExplosionSound;
    [SerializeField] private AudioClip bigExplosionSounds;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Enum to manage the different kinds of explosions, can be added if more explosions are required:
    public enum ExplosionTypes
    {
        BigExplosion, SmallExplosion, UfoExplosion
    }

    //MakeExplosion method takes in two parameters: Transform of where the explosion should occur
    //ExplosionType takes in an ExplosionTypes enum so it plays the appropriate explosion:
    public void MakeExplosion(Transform explosionLocation, ExplosionTypes explosionTypes)
    {
        //a Switch case to Instantiate the Explosion object on the specified location, then destroy it after 2 seconds:s
        switch (explosionTypes)
        {
            case ExplosionTypes.BigExplosion:
                audioSource.PlayOneShot(smallExplosionSound);
                Destroy(Instantiate(bigExplosion, explosionLocation.position, Quaternion.identity), 2f);
                break;

            case ExplosionTypes.SmallExplosion:
                Destroy(Instantiate(smallExplosion, explosionLocation.position, Quaternion.identity), 2f);
                break;

            case ExplosionTypes.UfoExplosion:
                audioSource.PlayOneShot(bigExplosionSounds);
                Destroy(Instantiate(ufoExplosion, explosionLocation.position, Quaternion.identity), 2f);
                break;
        }
    }
}