using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemDrop : MonoBehaviour
{
    public static ItemDrop Instance;
    //Powerups Prefabs are all dragged into this list, so a random powerup can be picked to spawn
    [SerializeField] private Powerup[] powerupList;

    //A chance int is implemented and changeable from Unity as different enemies have different dropRates
    //the lower the number, the higher the chance
    [SerializeField] private int chanceForNothing = 5;
    private List<int> powerupWeights;
    private int total;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        powerupWeights = powerupList.OrderByDescending(x => x.ItemDropWeight).Select(x => x.ItemDropWeight).ToList();
        powerupWeights.Insert(0, chanceForNothing);
        total = powerupWeights.Sum(x => x);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) DropItem();
    }

    private void DropItem()
    {
        var randomNumber = Random.Range(0, total);
        print("Random Number: " + randomNumber);

        for (int i = 0; i < powerupWeights.Count; i++)
        {
            if (randomNumber <= powerupWeights[i])
            {
                print("Award Weight: " + powerupWeights[i]);
                if (i == 0) return;
                Instantiate(powerupList[i - 1], transform.position, Quaternion.identity);
                return;
            }
            else
            {
                randomNumber -= powerupWeights[i];
            }
        }
    }
}