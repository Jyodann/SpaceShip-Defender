using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject[] powerupList;
    [SerializeField] private int chance = 5;

    public void DropItem()
    {
        //10% Chance to Drop an Item:
        if (Random.Range(0, chance) == 0)
        {
            Instantiate(powerupList[Random.Range(0, powerupList.Length)], transform.position, Quaternion.identity);
        }
    }
}