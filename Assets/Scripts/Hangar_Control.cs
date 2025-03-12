using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hangar_Control : MonoBehaviour
{   
    [Header ("SO_DATA")]
    public SO_Data data;
    public UpgradeCosts upgradeCosts;
    public Player_Sprites player_Sprites;
    
    public string sceneToLoad;

    [Header ("Components")]
    public Dialogue_Control dialogue_Control;
    public Hangar_Main_Panel_Control hangar_Main_Panel_Control;
    public GameObject dialoguePanel;
    public GameObject[] bilsDialogGO;


    void Start(){
        data = GameObject.Find("SO_DATA").GetComponent<SO_Data>();
        upgradeCosts = GameObject.Find("UpgradeCosts").GetComponent<UpgradeCosts>();
        player_Sprites = GameObject.Find("Player Sprites").GetComponent<Player_Sprites>();
    }

    public void resetAndSetDialogs(){
        // Set false every GO 
        hangar_Main_Panel_Control.setActive(hangar_Main_Panel_Control.getMainPanel(), false);
        

        foreach (var item in bilsDialogGO)
        {
            item.SetActive(false);
        }
        

        // Sets Bils's Dialog properly
        if(data.hangarArrivals == 0){
            dialoguePanel.SetActive(true);
            setSpeech(0, true);
        }

        else if(data.hangarArrivals == 1 && !data.playerVisitedHangar){
            dialoguePanel.SetActive(true);
            data.playerVisitedHangar = true;
            setSpeech(1, false);
        }

        else{
            hangar_Main_Panel_Control.setActive(hangar_Main_Panel_Control.getMainPanel(), true);
        
        }
    }


    private void setSpeech(int index, bool hasEvent){
        // Dialogue dial = bilsDialogGO[index].GetComponent<Dialogue>();
        dialogue_Control.setDialogue(bilsDialogGO[index]);
     
      
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
