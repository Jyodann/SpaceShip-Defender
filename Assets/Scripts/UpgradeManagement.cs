using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManagement : MonoBehaviour
{
    public int currentUpgradeIndex;
    public Text upgradeText;
    private FireBullets playerShip;

    public static List<UpgradeClass> upgrades = new List<UpgradeClass>()
    {
        //Upgrade Class Constructor: Name, Cost, CannonCount, FireRate, Damage per shot
        new UpgradeClass("BaseShip", 0, 1, 0.3f, 1),
        new UpgradeClass("+1 Cannon", 200, 2, 0.3f, 1),
        new UpgradeClass("+Fire Rate", 350, 2, 0.2f, 1),
        new UpgradeClass("+Damage", 500, 2, 0.2f, 3),
        new UpgradeClass("+1 Cannon", 750, 3, 0.2f, 3),
        new UpgradeClass("+Fire Rate", 1000, 3, 0.1f, 3),
        new UpgradeClass("+Damage", 1250, 3, 0.1f, 5),
        new UpgradeClass("+1 Cannon", 1500, 4, 0.1f, 5),
        new UpgradeClass("+Fire Rate", 2000, 4, 0.05f, 5),
        new UpgradeClass("+1 Cannon", 2500, 5, 0.05f, 5)
    };

    private void Start()
    {
        upgradeText.text = string.Empty;
        playerShip = FindObjectOfType<FireBullets>();
        currentUpgradeIndex = 0;
        ApplyUpgrade(upgrades[0]);

    }

    private void Update()
    {
        if (currentUpgradeIndex == upgrades.Count) return;
        if (upgrades[currentUpgradeIndex].UpgradeCost > GameManager.instance.Coins) return;
        var nextUpgrade = upgrades[currentUpgradeIndex];
        upgradeText.text = $"Press B To Upgrade: {nextUpgrade.UpgradeName}";
        if (!Input.GetKeyDown(KeyCode.B)) return;
        print("B Clicked");
        ApplyUpgrade(nextUpgrade);
    }

    private void ApplyUpgrade(UpgradeClass upgrade)
    {
        playerShip.damageDealt = upgrade.DamageCount;
        playerShip.fireRate = upgrade.FireRate;
        playerShip.cannonCount = upgrade.CannonCount;
        currentUpgradeIndex++;
        GameManager.instance.AddCoins(-upgrade.UpgradeCost);
        upgradeText.text = string.Empty;
    }
}