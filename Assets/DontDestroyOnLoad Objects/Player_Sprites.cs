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

    public void SetPlayerNextPlane()
    {
        myPlayerSprite = GetNextPlayerPlane();
    }

    public Sprite GetNextPlayerPlane()
    {
        Sprite nextPlayerPlaneSprite = myPlayerSprite;


        for (int i = 0; i < myPlayerPlanes.Length - 1; i++)
        {
            
            if ((myPlayerPlanes[i] == myPlayerSprite) && (myPlayerPlanes[i + 1] != null))
            {
                nextPlayerPlaneSprite = myPlayerPlanes[i+1];
                break;
            }
        }

        return nextPlayerPlaneSprite;
    }


}
