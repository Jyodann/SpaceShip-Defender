using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    //Powerups Prefabs are all dragged into this list, so a random powerup can be picked to spawn
    [SerializeField] private GameObject[] powerupList;
    //A chance int is implemented and changeable from Unity as different enemies have different dropRates
    //the lower the number, the higher the chance
    [SerializeField] private int chance = 5;

    public void DropItem()
    {
        //Random chance generator, if it hits zero, an item is dropped
        if (Random.Range(0, chance) == 0)
        {
            //Randomly picks a powerup from the powerup list and instantiates it
            Instantiate(powerupList[Random.Range(0, powerupList.Length)], transform.position, Quaternion.identity);
        }
    }
}