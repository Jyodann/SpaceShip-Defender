using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject[] powerupList;

    public void DropItem()
    {
        //10% Chance to Drop an Item:
        if (Random.Range(0, 11) == 1)
        {
            Instantiate(powerupList[Random.Range(0, powerupList.Length)], transform.position, Quaternion.identity);
        }
    }
}