using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullets : MonoBehaviour
{
    [SerializeField] private GameObject bulletObject;
    [SerializeField] private Transform mainCannon;
    [SerializeField] private Transform leftCannon;
    [SerializeField] private Transform rightCannon;
    [SerializeField] private Transform extremeLeftCannon;
    [SerializeField] private Transform extremeRightCannon;

    [SerializeField] private float bulletSpeed = 100f;

    public float fireRate = 0.5f;
    [Range(1, 5)] public int CannonCount = 1;
    public int DamageDealt = 1;

    public bool canFire = true;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (canFire)
        {
            canFire = false;
            switch (CannonCount)
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

            Invoke("EnableFire", fireRate);
        }
    }

    private void FireCannon(Transform cannonLocation)
    {
        GameObject mainbulletClone = Instantiate(bulletObject, cannonLocation.position, transform.rotation);
        mainbulletClone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * bulletSpeed);
    }

    private void EnableFire()
    {
        canFire = true;
    }
}