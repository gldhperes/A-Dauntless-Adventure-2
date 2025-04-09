using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "UpgradeCosts", menuName = "Create UpgradeCosts", order = 3)]
public class UpgradeCosts : MonoBehaviour
{   
    public int maxItensSold;

    [Header("Plane")]
    public int fuselagemCost;
    public bool fuselagemSold;
    public int fuselagemNextCost;


    [Header("Bomb ")]
    public int raioCost;
    public float raioUpgrade;
    public bool raioSold;
    public int raioInitialCost;
    public int raioNextCost;

    [Header("Speed")]
    public int speedCost;
    public float speedUpgrade;
    public bool speedSold;
    public int speedNextCost;

    [Header("Fire rate")]
    public int firerateCost;
    public float firerateUpgrade;
    public bool firerateSold;
    public int firerateNextCost;


    private Dictionary<UpgradeType, int> UpgradesNextCost;
  
    void Start()
    {
        UpgradesNextCost = new ()
        {
            { UpgradeType.NextPlane, fuselagemNextCost },
            { UpgradeType.Bomb, raioNextCost },
            { UpgradeType.Speed, speedNextCost },
            { UpgradeType.FireRate, firerateNextCost },
        };
        
        DontDestroyOnLoad(this.gameObject);
    }

    public int GetNextUpgrade(UpgradeType upgradeType)
    {
        return UpgradesNextCost[upgradeType];
    }

}
