using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [CreateAssetMenu(fileName = "SO_DATA", menuName = "Create SO_DATA", order = 1)]
public class SO_Data : MonoBehaviour
{   
    [Header("Game Level")]
    [SerializeField]
    private int myGameLevel;
    public int gameLevel
    {
        get { return this.myGameLevel; }
        set { myGameLevel = value; }
    }

    [SerializeField]
    private int myInitGameLevel;
    public int initGameLevel
    {
        get { return this.myInitGameLevel; }
    }

    [Header("Hangar Arrivals")]
    [SerializeField]
    private int myHangarArrivals;
    public int hangarArrivals
    {
        get { return this.myHangarArrivals; }
        set { myHangarArrivals = value; }
    }

    [SerializeField]
    private int myInitHangarArrivals;
    public int initHangarArrivals
    {
        get { return this.myInitHangarArrivals; }
    }

    [SerializeField]
    private bool myPlayerVisitedHangar;
    public bool playerVisitedHangar
    {
        get { return this.myPlayerVisitedHangar; }
        set { myPlayerVisitedHangar = value; }
    }

    [SerializeField]
    private bool myInitPlayerVisitedHangar;
    public bool initPlayerVisitedHangar
    {
        get { return this.myInitPlayerVisitedHangar; }
    }


    [Header("Player Level")]
    [SerializeField]
    private int myPlayerLevel;
    public int playerLevel
    {
        get { return this.myPlayerLevel; }
        set { myPlayerLevel = value; }
    }

    [SerializeField]
    private int myInitPlayerLevel;
    public int initPlayerLevel
    {
        get { return this.myInitPlayerLevel; }
    }
    

    [Header("Upgrade")]
    [SerializeField]
    private int myUpgradePoints;
    public int upgradePoints
    {
        get { return this.myUpgradePoints; }
        set { myUpgradePoints = value; }
    }

    [SerializeField]
    private int myInitUpgradePoints;
    public int initUpgradePoints
    {
        get { return this.myInitUpgradePoints; }
       
    }
    

    [Header("Life")]
    [SerializeField]
    private int myPlayerLife;
    public int playerLife
    {
        get { return this.myPlayerLife; }
        set { myPlayerLife = value; }
    }

    [SerializeField]
    private int myInitPlayerLife;
    public int initPlayerLife
    {
        get { return this.myInitPlayerLife; }
    }


    [Header("Speed")]
    [SerializeField]
    private float myPlayerSpeed;
    public float playerSpeed
    {
        get { return this.myPlayerSpeed; }
        set { myPlayerSpeed = value; }
    }

    [SerializeField]
    private float myInitPlayerSpeed;
    public float initPlayerSpeed
    {
        get { return this.myInitPlayerSpeed; }
    }
    

    [Header("Damage")]
    [SerializeField]
    private int myPlayerDamage;
    public int playerDamage
    {
        get { return this.myPlayerDamage; }
        set { myPlayerDamage = value; }
    }

    [SerializeField]
    private int myInitPlayerDamage;
    public int initPlayerDamage
    {
        get { return this.myInitPlayerDamage; }
    }


    [Header("Firerate")]
    [SerializeField]
    private float myPlayerFireRate;
    public float playerFireRate
    {
        get { return this.myPlayerFireRate; }
        set { myPlayerFireRate = value; }
    }

    [SerializeField]
    private float myInitPlayerFireRate;
    public float initPlayerFireRate
    {
        get { return this.myInitPlayerFireRate; }
    }


    [Header("Bomb")]
    [SerializeField]
    private float myPlayerBombArea;
    public float playerBombArea
    {
        get { return this.myPlayerBombArea; }
        set { myPlayerBombArea = value; }
    }

    [SerializeField]
    private float myPlayerInitBombArea;
    public float playerInitBombArea
    {
        get { return this.myPlayerInitBombArea; }
    }

    [SerializeField]
    private bool myPlayerBombEnable;
    public bool playerBombEnable
    {
        get { return this.myPlayerBombEnable; }
        set { myPlayerBombEnable = value; }
    }

    [SerializeField]
    private bool myInitPlayerBombEnable;
    public bool initPlayerBombEnable
    {
        get { return this.myInitPlayerBombEnable; }
    }

    void Start(){
        DontDestroyOnLoad(this.gameObject);
    }
}
   
