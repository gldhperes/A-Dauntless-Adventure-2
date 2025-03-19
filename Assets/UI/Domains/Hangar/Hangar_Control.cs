using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Hangar_Control : MonoBehaviour
{   
    [Header ("SO_DATA")]
    public SO_Data data;
    public UpgradeCosts upgradeCosts;
    public Player_Sprites player_Sprites;
    
    public string sceneToLoad;

    [Header("Components")] [SerializeField]
    private UIDocument uiDocument;
    private VisualElement screen;


    void Start(){
        data = GameObject.Find("SO_DATA").GetComponent<SO_Data>();
        upgradeCosts = GameObject.Find("UpgradeCosts").GetComponent<UpgradeCosts>();
        player_Sprites = GameObject.Find("Player Sprites").GetComponent<Player_Sprites>();
        uiDocument = GetComponent<UIDocument>();
        screen = uiDocument.rootVisualElement;
    }
    

    // ======================================================
    // BOTOES ===============================================
    // ======================================================

    public void setBlue(){ 
        setPlayerSprite(player_Sprites.getBluePlanes); 
    }

    public void setGreen(){ 
        setPlayerSprite(player_Sprites.getGreenPlanes); 
    }
    public void setRed(){ 
        setPlayerSprite(player_Sprites.getRedPlanes); 
    }
    public void setYellow(){ 
        setPlayerSprite(player_Sprites.getYellowPlanes); 
    }

    private void setPlayerSprite(Sprite[] sprites){

        player_Sprites.playerPlanes = sprites;
        player_Sprites.playerSprite = sprites[0];
        
        callSceneLoader();
    }

    public void callSceneLoader(){
        string scene = getSceneToLoad();
        GameLootLoading.LoadScene(scene);
    }

    private string getSceneToLoad(){
        string faseToLoad = (this.sceneToLoad);
        return faseToLoad;
    }
}
