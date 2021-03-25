using System.Collections.Generic;
using UnityEngine;

public class PowerupDurationManager : Singleton<PowerupDurationManager>
{
    public bool triggerCheck;
    private readonly Dictionary<Powerup, float> timings = new Dictionary<Powerup, float>();

    // Start is called before the first frame update
    private void Start()
    {
        foreach (var powerup in ItemDrop.Instance.powerupList) timings[powerup] = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (var timing in timings)
            {
                timing.Key.ResetPowerupEffect();
            }
        }
        if (triggerCheck)
        {
        }
    }
}