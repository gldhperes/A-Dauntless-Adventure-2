using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "UpgradeCosts", menuName = "Create UpgradeCosts", order = 3)]
public class UpgradeCosts : MonoBehaviour
{
    [Header("Plane")]
    public int fuselagemCost;


    [Header("Bomb ")]
    public int raioCost;
    public float raioUpgrade;

    

    [Header("Speed")]
    public int speedCost;
    public float speedUpgrade;


    [Header("Fire rate")]
    public int firerateCost;
    public float firerateUpgrade;
  
 
    private Dictionary<UpgradeType, int> UpgradesNextCost;

    public static UpgradeCosts Instance;

    void Awake()
    {
        // Se já existe uma instância diferente, destrói esse novo
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Define como instância única
        Instance = this;

        // Impede que seja destruído ao trocar de cena
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
      
    }

    public int GetNextUpgrade(UpgradeType upgradeType)
    {
        return UpgradesNextCost[upgradeType];
    }

}
