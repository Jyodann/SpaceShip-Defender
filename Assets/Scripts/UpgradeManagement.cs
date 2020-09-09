using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManagement : MonoBehaviour
{
    /// <summary>
    /// Upgrade Manager is used to change the properties of the ship based on the upgrade Classes Provided:
    /// </summary>

    //Keep current Index of upgrade based on upgradeList:
    public int currentUpgradeIndex;

    //References a Text, which is childed to the plane, so as to let the player know they can upgrade:
    [SerializeField] private Text upgradeText;

    //Gets a reference to the playerObject:
    private FireBullets playerShip;

    //A list of upgrades that can be appended if required:
    private List<UpgradeClass> upgrades = new List<UpgradeClass>()
    {
        //Upgrade Class Constructor: Name, Cost, CannonCount, FireRate, Damage per shot
        new UpgradeClass("BaseShip", 0, 1, 0.3f, 1),
        new UpgradeClass("+1 Cannon", 200, 2, 0.3f, 1),
        new UpgradeClass("+Fire Rate", 350, 2, 0.2f, 1),
        new UpgradeClass("+Damage", 500, 2, 0.2f, 3),
        new UpgradeClass("+1 Cannon", 1000, 3, 0.2f, 3),
        new UpgradeClass("+Fire Rate", 1500, 3, 0.1f, 3),
        new UpgradeClass("+Damage", 2000, 3, 0.1f, 5),
        new UpgradeClass("+1 Cannon", 2500, 4, 0.1f, 5),
        new UpgradeClass("+Fire Rate", 3500, 4, 0.05f, 5),
        new UpgradeClass("+1 Cannon", 5000, 5, 0.05f, 5)
    };

    private void Start()
    {
        //Sets HUD to be a blank, since no upgrades are purchasable:
        upgradeText.text = string.Empty;
        //Finds the only playShip object based on Firebullets script:
        playerShip = FindObjectOfType<FireBullets>();
        //Applies first upgrade to ship:
        ApplyUpgrade(upgrades[currentUpgradeIndex]);
    }

    private void Update()
    {
        //Detects if they have reached the final upgrade:
        if (currentUpgradeIndex == upgrades.Count) return;
        //Detects if the nextUpgrade is possible based on amount of money left:
        if (upgrades[currentUpgradeIndex].UpgradeCost > GameManager.instance.Coins) return;
        //If it is possible, set the Text the have the next upgrade name:
        var nextUpgrade = upgrades[currentUpgradeIndex];
        if (GameManager.playerControlMode != GameManager.ControlMode.MobileInput)
        {
            upgradeText.text = $"Press E To Upgrade: {nextUpgrade.UpgradeName}";
        }
        else
        {
            upgradeText.text = $"Tap Anywhere to Upgrade: {nextUpgrade.UpgradeName}";
        }

        //Check to see if player Clicks B to buy the upgrade:
        ApplyUpgrade(nextUpgrade);
    }

    //Apply upgrade takes in the Upgrade class and applies it to the ship:
    private void ApplyUpgrade(UpgradeClass upgrade)
    {
        playerShip.damageDealt = upgrade.DamageCount;
        playerShip.fireRate = upgrade.FireRate;
        playerShip.cannonCount = upgrade.CannonCount;
        //Changes upgradeIndex to next one:
        currentUpgradeIndex++;
        //Deducts coins from the game manager:
        GameManager.instance.AddCoins(-upgrade.UpgradeCost);
        //Upgrade text changes to empty string as no upgrades are possible:
        upgradeText.text = string.Empty;
    }
}