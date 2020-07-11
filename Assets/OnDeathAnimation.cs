using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathAnimation : MonoBehaviour
{
    [SerializeField] private GameObject smallExplosion;
    [SerializeField] private GameObject bigExplosion;

    public void MakeExplosion(Transform explosionLocation, bool isBigExplosion)
    {
        if (isBigExplosion)
        {
            Destroy(Instantiate(bigExplosion, explosionLocation.position, Quaternion.identity), 2f);
        }
        else
        {
            Destroy(Instantiate(smallExplosion, explosionLocation.position, Quaternion.identity), 2f);
        }
    }
}