using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// [CreateAssetMenu(fileName = "SO_DATA", menuName = "Create SO_DATA", order = 1)]
public class SO_Data : MonoBehaviour
{
    public Action<int> OnMoneyChanged;
    private Dictionary<UpgradeType, Action> functionsDictionary = new();




    #region PLAYER_VARIABLES

    [Header("Game Level")]
    [SerializeField]
    private int myGameLevel;

    public int gameLevel
    {
        get { return this.myGameLevel; }
        set { myGameLevel = value; }
    }

    [Header("Player Level")]
    [SerializeField]
    private int myPlayerLevel;

    public int playerLevel
    {
        get { return this.myPlayerLevel; }
        set { myPlayerLevel = value; }
    }

    [Header("Upgrade")][SerializeField] private int myUpgradePoints;

    public int upgradePoints
    {
        get { return this.myUpgradePoints; }
        set { myUpgradePoints = value; }
    }

    [Header("Life")][SerializeField] private int myPlayerLife;

    public int playerLife
    {
        get { return this.myPlayerLife; }
        set { myPlayerLife = value; }
    }

    [Header("Speed")][SerializeField] private float myPlayerSpeed;

    public float playerSpeed
    {
        get { return this.myPlayerSpeed; }
        set { myPlayerSpeed = value; }
    }

    [Header("Damage")][SerializeField] private int myPlayerDamage;

    public int playerDamage
    {
        get { return this.myPlayerDamage; }
        set { myPlayerDamage = value; }
    }

    [Header("Firerate")][SerializeField] private float myPlayerFireRate;

    public float playerFireRate
    {
        get { return this.myPlayerFireRate; }
        set { myPlayerFireRate = value; }
    }

    [Header("Bomb")][SerializeField] private float myPlayerBombArea;

    public float playerBombArea
    {
        get { return this.myPlayerBombArea; }
        set { myPlayerBombArea = value; }
    }

    [SerializeField] private bool myPlayerBombEnable;

    public bool playerBombEnable
    {
        get { return this.myPlayerBombEnable; }
        set { myPlayerBombEnable = value; }
    }

    [SerializeField] private bool myInitPlayerBombEnable;

    public bool initPlayerBombEnable
    {
        get { return this.myInitPlayerBombEnable; }
    }

    #endregion PLAYER_VARIABLES

    public static SO_Data Instance;

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
        functionsDictionary = new Dictionary<UpgradeType, Action>
        {
            { UpgradeType.NextPlane, UpgradeNextPlane },
            { UpgradeType.Bomb, UpgradeBomb },
            { UpgradeType.Speed, UpgradeSpeed },
            { UpgradeType.FireRate, UpgradeFirerate },
        };
    }

    #region PRIVATE_UPGRADE_FUNCTIONS

    private void UpgradeNextPlane()
    {
        this.playerLevel += 1;
        this.playerLife += 1;
        this.playerDamage += 1;
    }

    private void UpgradeBomb()
    {
        if (!playerBombEnable)
        {
            playerBombEnable = true;
        }

        playerBombArea += .75f;
    }

    private void UpgradeSpeed()
    {
        this.playerSpeed += 2f;
    }

    private void UpgradeFirerate()
    {
        this.playerFireRate += .1f;
    }

    #endregion PRIVATE_UPGRADE_FUNCTIONS


    public bool TryPurchase(UpgradeType upgradeType, int cost)
    {
        if (upgradePoints >= cost)
        {
            upgradePoints -= cost;

            // Disparar eventos
            OnMoneyChanged?.Invoke(upgradePoints);

            // Dispara a ação de Upgrade
            functionsDictionary[upgradeType]?.Invoke();

            return true;
        }

        return false;
    }
}

public enum UpgradeType
{
    NextPlane,
    Bomb,
    Speed,
    FireRate,
}

public static class UpgradeNames
{
    public static readonly Dictionary<UpgradeType, string> upgradesDict = new()
    {
        { UpgradeType.NextPlane, "NextPlane" },
        { UpgradeType.Bomb, "Bomb" },
        { UpgradeType.Speed, "Speed" },
        { UpgradeType.FireRate, "FireRate" },
    };
}