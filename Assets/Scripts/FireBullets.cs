using System.Collections;
using UnityEngine;

public class FireBullets : MonoBehaviour
{
    /// <summary>
    /// This class is mainly for handling bullet firing from the playerObject
    /// </summary>

    //Takes in a bullet prefab as the bullet to be shot:
    [SerializeField] private GameObject bulletObject;

    //All these transforms are empty game objects childed to the ship in their respective locations
    //These serialized variables are not public as they do not need to be referenced anywhere else,
    //but need to have a reference from the editor.

    [SerializeField] private Transform mainCannon;
    [SerializeField] private Transform leftCannon;
    [SerializeField] private Transform rightCannon;
    [SerializeField] private Transform extremeLeftCannon;
    [SerializeField] private Transform extremeRightCannon;

    //AudioClip that can be changed from inspector if required. This sound plays when a bullet is fired:
    [SerializeField] private AudioClip lazerShot;

    //Bullet speed can be changed from inspector, affects how fast the bullet flies from the ship.
    [SerializeField] private float bulletSpeed = 100f;

    //FireRate float will affect the number of bullets shot per second.
    public float fireRate = 0.5f;

    //Affects number of cannons currently firing based on upgrade:
    [Range(1, 5)] public int cannonCount = 1;

    //Affects amount of damage dealt per bullet based on upgrade:
    public int damageDealt = 1;

    //Stores an audioSource as a global variable so it can play the LazerShot sound
    private AudioSource audioSource;

    private void Start()
    {
        //Caches a reference to the audioSource component so it can be used afterwards:
        audioSource = GetComponent<AudioSource>();
        //Starts the fire() Coroutine so that the player can start Firing Bullets:
        StartCoroutine(Fire());
    }

    /// <summary>
    /// Fire() coroutine is responsible for regulating the bullet fire rate.
    /// Coroutine used here as the rate of fire is easier to manage with yeild return waitForSeconds
    /// compared to using Update()
    /// </summary>
    /// <returns></returns>
    private IEnumerator Fire()
    {
        while (true)
        {
            //If game is paused, disallows any other action to occur:
            while (GameManager.instance.isPaused)
            {
                yield return null;
            }
            //If Fire1 (MouseLeft) is held, then it starts to fire:
            if (Input.GetButton("Fire1"))
            {
                audioSource.PlayOneShot(lazerShot, 0.5f);
                //Fires from respective cannons depending on how many there are in the current upgrade:
                switch (cannonCount)
                {
                    case 1:
                        FireCannon(mainCannon.transform);
                        break;

                    case 2:
                        FireCannon(leftCannon.transform);
                        FireCannon(rightCannon.transform);
                        break;

                    case 3:
                        FireCannon(mainCannon.transform);
                        FireCannon(leftCannon.transform);
                        FireCannon(rightCannon.transform);
                        break;

                    case 4:

                        FireCannon(leftCannon.transform);
                        FireCannon(rightCannon.transform);
                        FireCannon(extremeLeftCannon.transform);
                        FireCannon(extremeRightCannon.transform);
                        break;

                    case 5:
                        FireCannon(mainCannon.transform);
                        FireCannon(leftCannon.transform);
                        FireCannon(rightCannon.transform);
                        FireCannon(extremeLeftCannon.transform);
                        FireCannon(extremeRightCannon.transform);
                        break;

                    default:
                        break;
                }
            }
            //returns waitForSecondsRealTime as fireRate is counted in realSeconds, and this method will wait for the seconds to end
            //before allowing another call:
            yield return new WaitForSecondsRealtime(fireRate);
        }
    }

    //Fire cannon Method accepts a cannon location, and instantiates a bullet prefab in that location
    private void FireCannon(Transform cannonLocation)
    {
        //TransformDirection translates the current ship's facing direction (local) to a
        //world vector, which allows bullets to fly from where the ship is facing:

        Instantiate(bulletObject, cannonLocation.position, transform.rotation).GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * bulletSpeed);
    }
}