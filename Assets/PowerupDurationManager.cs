using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDurationManager : Singleton<PowerupDurationManager>
{
    public bool triggerCheck;
    private Dictionary<string, float> timings = new Dictionary<string, float>() ;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var powerup in ItemDrop.Instance.powerupList)
        {
            timings[powerup.name] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerCheck)
        {
            
        }
    }
}
