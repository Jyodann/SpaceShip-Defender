public class UpgradeClass
{
    /// <summary>
    /// This Upgrade class is for creating upgrades dynamically, it is used in the UpgradeManagement.cs File
    /// </summary>
    public string UpgradeName { get; set; }

    public int UpgradeCost { get; set; }

    public int CannonCount { get; set; }

    public float FireRate { get; set; }

    public int DamageCount { get; set; }

    //Default Constructor for making new upgrades, this is so the game can be built upon:
    public UpgradeClass(string upgradeName, int upgradeCost, int cannonCount, float fireRate, int damageCount)
    {
        UpgradeName = upgradeName;
        UpgradeCost = upgradeCost;
        CannonCount = cannonCount;
        FireRate = fireRate;
        DamageCount = damageCount;
    }
}