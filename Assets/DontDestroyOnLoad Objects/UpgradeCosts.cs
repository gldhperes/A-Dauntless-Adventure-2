using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "UpgradeCosts", menuName = "Create UpgradeCosts", order = 3)]
public class UpgradeCosts : MonoBehaviour
{   
    public int maxItensSold;

    [Header("Plane")]
    public int fuselagemCost;
    public int fuselagemSold;
    public int fuselagemNextCost;


    [Header("Bomb ")]
    public int raioCost;
    public float raioUpgrade;
    public int raioSold;
    public int raioInitialCost;
    public int raioNextCost;

    [Header("Speed")]
    public int speedCost;
    public float speedUpgrade;
    public int speedSold;
    public int speedNextCost;

    [Header("Fire rate")]
    public int firerateCost;
    public float firerateUpgrade;
    public int firerateSold;
    public int firerateNextCost;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
