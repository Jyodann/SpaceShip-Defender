using System.Collections;
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
        new UpgradeClass("+1 Cannon", 100, 2, 0.3f, 1),
        new UpgradeClass("+Fire Rate", 150, 2, 0.2f, 1),
        new UpgradeClass("+Damage", 200, 2, 0.2f, 3),
        new UpgradeClass("+1 Cannon", 250, 3, 0.2f, 3),
        new UpgradeClass("+Fire Rate", 300, 3, 0.1f, 3),
        new UpgradeClass("+Damage", 350, 3, 0.1f, 5),
        new UpgradeClass("+1 Cannon", 400, 4, 0.1f, 5),
        new UpgradeClass("+Fire Rate", 350, 4, 0.05f, 5),
        new UpgradeClass("+1 Cannon", 350, 5, 0.1f, 5)
    };

    private void Start()
    {
        upgradeText.text = string.Empty;
        playerShip = FindObjectOfType<FireBullets>();
        currentUpgradeIndex = 0;
        ApplyUpgrade(upgrades[0]);
        StartCoroutine(CheckForUpgrade());
    }

    private IEnumerator CheckForUpgrade()
    {
        while (true)
        {
            print("CheckUpgrade");
            //Check if there is another upgrade available
            if (currentUpgradeIndex + 1 != upgrades.Count)
            {
                if (upgrades[currentUpgradeIndex + 1].UpgradeCost <= GameManager.Instance.Coins)
                {
                    var nextUpgrade = upgrades[currentUpgradeIndex + 1];
                    upgradeText.text = $"Press B To Upgrade: {nextUpgrade.UpgradeName}";
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        print("B Clicked");
                        ApplyUpgrade(nextUpgrade);
                    }
                }
                yield return null;
            }
            else
            {
                break;
            }
        }
    }

    private void ApplyUpgrade(UpgradeClass upgrade)
    {
        playerShip.damageDealt = upgrade.DamageCount;
        playerShip.fireRate = upgrade.FireRate;
        playerShip.cannonCount = upgrade.CannonCount;
        currentUpgradeIndex++;
        GameManager.Instance.AddCoins(-upgrade.UpgradeCost);
        upgradeText.text = string.Empty;
    }
}