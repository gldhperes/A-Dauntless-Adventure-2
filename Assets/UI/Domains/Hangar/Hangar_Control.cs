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
    
    void Start(){
        data = GameObject.Find("SO_DATA").GetComponent<SO_Data>();
        upgradeCosts = GameObject.Find("UpgradeCosts").GetComponent<UpgradeCosts>();
        player_Sprites = GameObject.Find("Player Sprites").GetComponent<Player_Sprites>();
    }
    
    


}
