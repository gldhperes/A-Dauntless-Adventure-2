using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Buttons_Behaviour : MonoBehaviour
{
    [Header("Panels")]
    public GameObject initialPanel;
    public GameObject mainPanel;
    public GameObject newPanel;
    public GameObject optionsPanel;
    public GameObject comoJogarPanel;

    [Header ("SO_DATA")]
    public SO_Data data;
    public UpgradeCosts upgradeCosts;
    public Player_Sprites player_Sprites;
   
    void Start(){
        data = GameObject.Find("SO_DATA").GetComponent<SO_Data>();
        upgradeCosts = GameObject.Find("UpgradeCosts").GetComponent<UpgradeCosts>();
        player_Sprites = GameObject.Find("Player Sprites").GetComponent<Player_Sprites>();
        openInitialPanel();
    }

    void Update(){
        if(initialPanel.activeSelf && Input.anyKeyDown){
            openMainPanel();
        }
    }

    public void openInitialPanel(){
        initialPanel.SetActive(true);
        mainPanel.SetActive(false);
        newPanel.SetActive(false);
        optionsPanel.SetActive(false);
        comoJogarPanel.SetActive(false);
    }

    public void openMainPanel(){
        initialPanel.SetActive(false);
        mainPanel.SetActive(true);
        newPanel.SetActive(false);
        optionsPanel.SetActive(false);
        comoJogarPanel.SetActive(false);
    }

    // NEW GAME PANEL
    public void opeNewPanel(){
        mainPanel.SetActive(false);
        newPanel.SetActive(true);
        optionsPanel.SetActive(false);
         comoJogarPanel.SetActive(false);
    }

    public void startNewGame(string scene){   
        resetPlayerData(); 
        resetPlayerSprites();
        resetUpgradeCosts();
        GameLootLoading.LoadScene(scene);
    }

    private void resetPlayerData(){
        data.gameLevel = data.initGameLevel;

        data.hangarArrivals = data.initHangarArrivals;

        data.playerVisitedHangar = data.initPlayerVisitedHangar;

        data.playerLevel = data.initPlayerLevel;

        data.upgradePoints = data.initUpgradePoints;

        data.playerLife = data.initPlayerLife;

        data.playerSpeed = data.initPlayerSpeed;

        data.playerDamage = data.initPlayerDamage;

        data.playerFireRate = data.initPlayerFireRate;

        data.playerBombArea = data.playerInitBombArea;

        data.playerBombEnable = data.initPlayerBombEnable;

        // Checa se foram mesmo resetados
        if(data.gameLevel != data.initGameLevel) resetPlayerData();

        if(data.hangarArrivals != data.initHangarArrivals) resetPlayerData();

        if(data.playerVisitedHangar != data.initPlayerVisitedHangar) resetPlayerData();

        if(data.playerLevel != data.initPlayerLevel) resetPlayerData();

        if(data.upgradePoints != data.initUpgradePoints) resetPlayerData();

        if(data.playerLife != data.initPlayerLife) resetPlayerData();

        if( data.playerSpeed != data.initPlayerSpeed) resetPlayerData();

        if(data.playerDamage != data.initPlayerDamage) resetPlayerData();

        if(data.playerFireRate != data.initPlayerFireRate) resetPlayerData();

        if(data.playerBombArea != data.playerInitBombArea) resetPlayerData();

        if(data.playerBombEnable != data.initPlayerBombEnable) resetPlayerData();


    }

    private void resetPlayerSprites(){
        player_Sprites.playerPlanes = null;
        player_Sprites.playerSprite = null;

        if( (player_Sprites.playerPlanes != null) || (player_Sprites.playerSprite != null) ){
            resetPlayerSprites();
        }
    }

    private void resetUpgradeCosts(){
        // ERRO
        // upgradeCosts.fuselagemCost = 10;
        // upgradeCosts.fuselagemSold = 0;

        // upgradeCosts.raioCost = 10;
        // upgradeCosts.raioSold = 0;

        // upgradeCosts.velocidadeCost = 10;
        // upgradeCosts.velocidadeSold = 0;

        // upgradeCosts.firerateCost = 10;
        // upgradeCosts.firerateSold = 0;

        // if( (upgradeCosts.fuselagemCost != 10) || (upgradeCosts.fuselagemSold != 0) ){
        //     resetUpgradeCosts();
        // }

        // if( (upgradeCosts.raioCost != 10) || (upgradeCosts.raioSold != 0) ){
        //     resetUpgradeCosts();
        // }

        // if( (upgradeCosts.velocidadeCost != 10) || (upgradeCosts.velocidadeSold != 0) ){
        //     resetUpgradeCosts();
        // }

        // if( (upgradeCosts.firerateCost != 10) || (upgradeCosts.firerateSold != 0) ){
        //     resetUpgradeCosts();
        // }
    }

    public void openOptionsPanel(){
        mainPanel.SetActive(false);
        newPanel.SetActive(false);
        optionsPanel.SetActive(true);
        comoJogarPanel.SetActive(false);
    }

    public void openComoJogarPanel(){
        mainPanel.SetActive(false);
        newPanel.SetActive(false);
        optionsPanel.SetActive(false);
        comoJogarPanel.SetActive(true);
    }

}