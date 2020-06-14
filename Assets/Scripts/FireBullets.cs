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
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float bulletSpeed = 50f;

    // Start is called before the first frame update
    private void Start()
    {
    }

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
        GameObject mainbulletClone = Instantiate(bulletObject, mainCannon.position, transform.rotation);
        mainbulletClone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * bulletSpeed);

        GameObject rightBulletClone = Instantiate(bulletObject, rightCannon.position, transform.rotation);
        rightBulletClone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * bulletSpeed);

        GameObject leftBulletClone = Instantiate(bulletObject, leftCannon.position, transform.rotation);
        leftBulletClone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * bulletSpeed);
    }
}