using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeClass
{
    public string UpgradeName { get; set; }

    public int UpgradeCost { get; set; }

    public int CannonCount { get; set; }

    public float FireRate { get; set; }

    public int DamageCount { get; set; }

    public UpgradeClass(string upgradeName, int upgradeCost, int cannonCount, float fireRate, int damageCount)
    {
        UpgradeName = upgradeName;
        UpgradeCost = upgradeCost;
        CannonCount = cannonCount;
        FireRate = fireRate;
        DamageCount = damageCount;
    }
}