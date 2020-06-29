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

    [SerializeField] private float bulletSpeed = 100f;

    public float fireRate = 0.5f;
    [Range(1, 3)] public int CannonCount = 1;
    public int DamageDealt = 1;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            InvokeRepeating("Fire", 0f, fireRate);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            CancelInvoke();
        }
    }

    private void Fire()
    {
        switch (CannonCount)
        {
            case 1:
                GameObject mainbulletClone = Instantiate(bulletObject, mainCannon.position, transform.rotation);
                mainbulletClone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * bulletSpeed);
                break;

            case 2:
                GameObject rightBulletClone = Instantiate(bulletObject, rightCannon.position, transform.rotation);
                rightBulletClone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * bulletSpeed);

                GameObject leftBulletClone = Instantiate(bulletObject, leftCannon.position, transform.rotation);
                leftBulletClone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * bulletSpeed);
                break;

            case 3:
                GameObject mainbulletCloneUpgraded = Instantiate(bulletObject, mainCannon.position, transform.rotation);
                mainbulletCloneUpgraded.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * bulletSpeed);

                GameObject rightBulletCloneUpgraded = Instantiate(bulletObject, rightCannon.position, transform.rotation);
                rightBulletCloneUpgraded.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * bulletSpeed);

                GameObject leftBulletCloneUpgraded = Instantiate(bulletObject, leftCannon.position, transform.rotation);
                leftBulletCloneUpgraded.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * bulletSpeed);
                break;

            default:
                break;
        }
    }

    public void AttemptUpgrade()
    {
    }
}