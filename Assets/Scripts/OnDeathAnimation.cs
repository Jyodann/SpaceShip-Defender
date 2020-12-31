using UnityEngine;

public class OnDeathAnimation : MonoBehaviour
{
    //Enum to manage the different kinds of explosions, can be added if more explosions are required:
    public enum ExplosionTypes
    {
        BigExplosion,
        SmallExplosion,
        UfoExplosion
    }

    /// <summary>
    ///     Death Animation manager will be responsible for playing explosions on command.
    /// </summary>

    //These SerialisedFields are for setting the prefabs with a particleSystem in it that has the corresponding animation:
    [SerializeField] private GameObject smallExplosion;

    [SerializeField] private GameObject bigExplosion;
    [SerializeField] private GameObject ufoExplosion;

    //These two serialiseFields allow any audioClip to be swapped out so explosion sounds can be changed:
    [SerializeField] private AudioClip smallExplosionSound;

    [SerializeField] private AudioClip bigExplosionSounds;

    //Sets a global reference to audioSource so the sounds can be played:
    private AudioSource audioSource;

    private void Start()
    {
        //Caches the audioSource so it can be used later:
        audioSource = GetComponent<AudioSource>();
    }

    //MakeExplosion method takes in two parameters: Transform of where the explosion should occur
    //ExplosionType takes in an ExplosionTypes enum so it plays the appropriate explosion:
    public void MakeExplosion(Transform explosionLocation, ExplosionTypes explosionTypes)
    {
        //a Switch case to Instantiate the Explosion object on the specified location, then destroy it after 2 seconds:s
        switch (explosionTypes)
        {
            case ExplosionTypes.BigExplosion:
                //Plays the small explosionSound using audio source with a volume of 0.35:
                audioSource.PlayOneShot(smallExplosionSound, 0.35f);
                Destroy(Instantiate(bigExplosion, explosionLocation.position, Quaternion.identity), 2f);
                break;

            case ExplosionTypes.SmallExplosion:
                Destroy(Instantiate(smallExplosion, explosionLocation.position, Quaternion.identity), 2f);
                break;

            case ExplosionTypes.UfoExplosion:
                //Plays the large explosionSound using audio source with a volume of 1:
                audioSource.PlayOneShot(bigExplosionSounds, 1);
                Destroy(Instantiate(ufoExplosion, explosionLocation.position, Quaternion.identity), 2f);
                break;
        }
    }
}