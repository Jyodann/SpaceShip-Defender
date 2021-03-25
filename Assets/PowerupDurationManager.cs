using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class PowerupDurationManager : Singleton<PowerupDurationManager>
{
    [UsedImplicitly]
    public class TimingAndPowerupClass
    {
        public TimingAndPowerupClass(Powerup currentPowerup, float timing)
        {
            this.currentPowerup = currentPowerup;
            this.timing = timing;
        }

        public Powerup currentPowerup;
        public float timing;
    }
    
    private Dictionary<string, TimingAndPowerupClass> powerupDictionary = new Dictionary<string, TimingAndPowerupClass>();
    private List<string> powerupName;
    public List<float> debugTimings = new List<float>();
    
    // Start is called before the first frame update
    private void Start()
    {
        foreach (var powerup in ItemDrop.Instance.powerupList)
        {
            print(powerup.gameObject.name);
            powerupDictionary[$"{powerup.name}(Clone)"] = new TimingAndPowerupClass(powerup, 0);
        }
        foreach (var timing in powerupDictionary)
        {
            debugTimings.Add(timing.Value.timing);
        }

        powerupName = powerupDictionary.Keys.ToList();
        StartCoroutine(StartCountdown());
    }

    public void AddTiming(string currentPowerup, float timeToadd)
    {
        if (powerupDictionary[currentPowerup].timing == -1)
        {
            powerupDictionary[currentPowerup].timing += timeToadd + 1;
            return;
        }

        powerupDictionary[currentPowerup].timing += timeToadd;
    }

    IEnumerator StartCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            foreach (var timing in powerupName)
            {
                powerupDictionary[timing].timing--;
                powerupDictionary[timing].timing = Mathf.Clamp(powerupDictionary[timing].timing, -1, Mathf.Infinity);
                if (powerupDictionary[timing].timing != 0) continue;
                print("Reset: " + powerupDictionary[timing].currentPowerup.name);
                powerupDictionary[timing].currentPowerup.ResetPowerupEffect();
            }
        }
    }

    // Update is called once per frame
    #if UNITY_EDITOR
    private void Update()
    {
        for (int index = 0; index < powerupDictionary.Count; index++) {
            var item = powerupDictionary.ElementAt(index);
            var itemKey = item.Key;
            var itemValue = item.Value;

            debugTimings[index] = item.Value.timing;
        }
    }
    #endif
}