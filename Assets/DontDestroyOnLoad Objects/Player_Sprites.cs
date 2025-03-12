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

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    
}
