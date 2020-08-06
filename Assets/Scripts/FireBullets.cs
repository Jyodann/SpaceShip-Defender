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
    //why serialiseVariable?
    [SerializeField] private Transform mainCannon;
    [SerializeField] private Transform leftCannon;
    [SerializeField] private Transform rightCannon;
    [SerializeField] private Transform extremeLeftCannon;
    [SerializeField] private Transform extremeRightCannon;
    
    //Bullet speed can be changed from inspector, affects how fast the bullet flies from the ship. 
    [SerializeField] private float bulletSpeed = 100f;
    
    //FireRate float will affect the number of bullets shot per second.
    public float fireRate = 0.5f;
    
    [Range(1, 5)] public int cannonCount = 1;
    public int damageDealt = 1;
    
    private void Start()
    {
        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            while (GameManager.instance.IsPaused)
            {
                yield return null;
            }
            if (Input.GetButton("Fire1"))
            {
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
            yield return new WaitForSecondsRealtime(fireRate);
        }
    }

    private void FireCannon(Transform cannonLocation)
    {
        GameObject mainbulletClone = Instantiate(bulletObject, cannonLocation.position, transform.rotation);
        mainbulletClone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * bulletSpeed);
    }
}