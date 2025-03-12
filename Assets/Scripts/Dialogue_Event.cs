using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Event : MonoBehaviour
{
    public GameObject activePanel;
    public bool activePanelBool;


    public void activeEvent(){
        if(activePanel != null){
            toggleActiveAPanel(!activePanelBool);
        }
    }



    public void toggleActiveAPanel(bool toggle){
        this.activePanel.SetActive(!toggle);
    }

    public GameObject getActiveAPanel(){
        return this.activePanel;
    }

}
