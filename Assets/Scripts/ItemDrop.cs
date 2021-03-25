using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemDrop : Singleton<ItemDrop>
{
    
    //Powerups Prefabs are all dragged into this list, so a random powerup can be picked to spawn
    public Powerup[] powerupList;

    private List<int> powerupWeights;
    private int total;
    private void Awake()
    {
        powerupWeights = powerupList.OrderByDescending(x => x.ItemDropWeight).Select(x => x.ItemDropWeight).ToList();
        total = powerupWeights.Sum(x => x);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.L)) DropItem();
    }

    public void DropItem(Transform positionToSpawn, int enemyPercentChance)
    {
        var percentChance = Random.Range(0, 101);
        if (percentChance >= enemyPercentChance) return;
        var randomNumber = Random.Range(0, total);
        //print("Random Number: " + randomNumber);

        for (int i = 0; i < powerupWeights.Count; i++)
        {
            if (randomNumber <= powerupWeights[i])
            {
                //print("Award Weight: " + powerupWeights[i]);
                if (i == 0) return;
                Instantiate(powerupList[i - 1], positionToSpawn.position, Quaternion.identity);
                return;
            }
            else
            {
                randomNumber -= powerupWeights[i];
            }
        }
    }
}