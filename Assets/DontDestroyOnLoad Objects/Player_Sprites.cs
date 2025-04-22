using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sprites : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField]
    private Sprite[] myPlayerPlanes;
    public Sprite[] playerPlanes
    {
        get { return myPlayerPlanes; }
        set { myPlayerPlanes = value; }
    }

    [SerializeField]
    private Sprite myPlayerSprite;
    public Sprite playerSprite
    {
        get { return myPlayerSprite; }
        set { myPlayerSprite = value; }
    }

    public Sprite[] getBluePlanes;
    public Sprite[] getGreenPlanes;
    public Sprite[] getRedPlanes;
    public Sprite[] getYellowPlanes;

    public static Player_Sprites Instance;
    [SerializeField] private int spriteIndex = 0;

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

    public void SetPlayerNextPlane()
    {
        myPlayerSprite = GetNextPlayerPlane();
    }

    public Sprite GetNextPlayerPlane()
    {
        Sprite nextPlayerPlaneSprite = myPlayerSprite;


        for (int i = spriteIndex; i < myPlayerPlanes.Length - 1; i++)
        {

            if (myPlayerPlanes[i] == myPlayerSprite)
            {
                nextPlayerPlaneSprite = myPlayerPlanes[i + 1];

                if (spriteIndex + 1 >= myPlayerPlanes.Length -1)
                {
                    FindAnyObjectByType<HangarScreen>().AddLockedItemToContainer("NextPlaneUpgradeButton");
                }

                spriteIndex = i;
                break;
            }



        }

        return nextPlayerPlaneSprite;
    }





}
