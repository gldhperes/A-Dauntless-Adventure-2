using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "UpgradeCosts", menuName = "Create UpgradeCosts", order = 3)]
public class UpgradeCosts : MonoBehaviour
{   
    public int maxItensSold;

    [Header("Fuselagem")]
    public int fuselagemCost;
    public int fuselagemSold;
    public int fuselagemNextCost;


    [Header("Bomb Raio")]
    public int raioCost;
    public float raioUpgrade;
    public int raioSold;
    public int raioInitialCost;
    public int raioNextCost;

    [Header("Velocidade")]
    public int velocidadeCost;
    public float velocidadeUpgrade;
    public int velocidadeSold;
    public int velocidadeNextCost;

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
