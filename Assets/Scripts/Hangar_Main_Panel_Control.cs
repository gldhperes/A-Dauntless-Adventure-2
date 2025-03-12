using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Hangar_Main_Panel_Control : MonoBehaviour
{
    [Header ("SO_DATA")]
    public SO_Data data;
    public UpgradeCosts upgradeCosts;
    public Player_Sprites player_Sprites;

    [Header ("Panels GameObjects")]
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject comoJogarPanel;

    public Toggle mobileToogle;

    [Header ("Bloqueado Panels")]
    public GameObject planeBloqueado;
    public GameObject bombBloqueado;
    public GameObject bombDesbloqueado;
    public GameObject velocidadeBloqueado;
    public GameObject firerateBloqueado;


    [Header ("Player Stats")]
    // Player
    public Image playerImage;
    public TMP_Text playerLevelText;

    // Upgrade
    public TMP_Text upgradePointsTxt;

    // Max Life
    public TMP_Text maxLifeTxt;

    // Firerate
    public TMP_Text firerateTxt;

    public TMP_Text danoTxt;

    // movement Speed
    public TMP_Text movementSpeedTxt;

    // bombArea
    public TMP_Text bombAreaTxt;

    public Image playerNextPlane;

    [Header ("Upgrade Costs")]
    public TMP_Text fuselagemTxtCost;
    public TMP_Text velocidadeTxtCost;
    public TMP_Text bombRaioTxtCost;
    public TMP_Text bombRaioTxtInitialCost;
    public TMP_Text firerateTxtCost;
    public GameObject errorMsgTxt;

    [Header ("Upgrade Points")]
    public TMP_Text velocidadeTxtUpgrade;
    public TMP_Text bombRaioTxtUpgrade;
    public TMP_Text firerateTxtUpgrade;

    void Start(){
        data = GameObject.Find("SO_DATA").GetComponent<SO_Data>();
        upgradeCosts = GameObject.Find("UpgradeCosts").GetComponent<UpgradeCosts>();
        player_Sprites = GameObject.Find("Player Sprites").GetComponent<Player_Sprites>();

        closeAllPanels();
        desactiveLevelMaximoPanels();
        GetComponent<Hangar_Control>().resetAndSetDialogs();

        if (data.hangarArrivals < 1){ return; }

        openMainPanel();

        // Atualiza o painel de estatus do jogador
        updateStatsPanel();


        updatePlayerImages(false);
       
        setCosts();
        checkBombBought();
    }

    private void closeAllPanels(){
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        comoJogarPanel.SetActive(false);
    }

    // =====================
    // UPDATE ==============
    // =====================

    void Update(){
        if (data.hangarArrivals < 1){ return; }

        if( !(upgradeCosts.fuselagemSold < upgradeCosts.maxItensSold) ) setMaxUpdateReached(planeBloqueado);
        if ( !(upgradeCosts.raioSold < upgradeCosts.maxItensSold) ) setMaxUpdateReached(bombBloqueado);
        if( !(upgradeCosts.velocidadeSold < upgradeCosts.maxItensSold) ) setMaxUpdateReached(velocidadeBloqueado);
        if( !(upgradeCosts.firerateSold < upgradeCosts.maxItensSold) ) setMaxUpdateReached(firerateBloqueado);
    }


    public void setActive(GameObject obj, bool b){
        obj.SetActive(b);
    }

    public GameObject getMainPanel(){
        return this.mainPanel;
    }

    // ===========================================
    // PLAYER STATS ==============================
    // ===========================================
    public void updateStatsPanel()
    {
        setPlayerLvlTxt();
        setUpgradeTxt();
        setMaxLifeTxt();
        setDamageTxt();
        setFirerateTxt();
        setMovementSpeedTxt();
        setBombAreaTxt();
    }

    private void updatePlayerImages(bool b){
        setPlayerPlaneImage();
        setPlayerNextPlane(b);
    }

    public void setPlayerPlaneImage(){ // Atualiza a imagem do aviao no painel de status
        playerImage.sprite = player_Sprites.playerSprite;
    }

    public void setPlayerLvlTxt()
    {   
        playerLevelText.text = "";
        playerLevelText.text = data.playerLevel.ToString();
    }

    public void setUpgradeTxt()
    {
        upgradePointsTxt.text = "";
        upgradePointsTxt.text = data.upgradePoints.ToString();
    }

    public void setMaxLifeTxt()
    {
        maxLifeTxt.text = "";
        maxLifeTxt.text = data.playerLife.ToString();
    }

    public void setDamageTxt(){
        danoTxt.text = "";
        danoTxt.text = data.playerDamage.ToString();
    }
    public void setFirerateTxt()
    {
        firerateTxt.text = "";
        firerateTxt.text = data.playerFireRate.ToString();
    }

    public void setMovementSpeedTxt()
    {   
        movementSpeedTxt.text = "";
        movementSpeedTxt.text = data.playerSpeed.ToString();
    }

    public void setBombAreaTxt()
    {
        bombAreaTxt.text = "";
        bombAreaTxt.text = data.playerBombArea.ToString()+"^²";
    }

    // =====================================================
    // Player Costs to Upgrade
    // =====================================================
    private void setCosts(){

        if( (upgradeCosts.fuselagemSold >= upgradeCosts.maxItensSold) ) {
            setMaxUpdateReached(planeBloqueado);
        } else { 
            fuselagemTxtCost.text = upgradeCosts.fuselagemCost.ToString();
        }

        if ( (upgradeCosts.raioSold >= upgradeCosts.maxItensSold) ) {
            setMaxUpdateReached(bombBloqueado);
        } else { 
            velocidadeTxtCost.text = upgradeCosts.velocidadeCost.ToString();
            velocidadeTxtUpgrade.text = "+" + upgradeCosts.velocidadeUpgrade.ToString() + " Velocidade";
        }

        if( (upgradeCosts.velocidadeSold >= upgradeCosts.maxItensSold) ) {
            setMaxUpdateReached(velocidadeBloqueado);
        } else { 
            bombRaioTxtCost.text = upgradeCosts.raioCost.ToString();
            bombRaioTxtInitialCost.text = upgradeCosts.raioInitialCost.ToString();
            bombRaioTxtUpgrade.text = "+" + upgradeCosts.raioUpgrade.ToString() + " Área";         
        }

        if( (upgradeCosts.firerateSold >= upgradeCosts.maxItensSold) ) {
            setMaxUpdateReached(firerateBloqueado);
        } else{
            firerateTxtCost.text = upgradeCosts.firerateCost.ToString();
            firerateTxtUpgrade.text = "-" + upgradeCosts.firerateUpgrade.ToString() + " Firerate";
        }

    }


    public void setPlayerNextPlane(bool updatePlayerSprite){
        
        // Faz uma lista de avioes do jogador
        List<Sprite> playerPlanes = new List<Sprite>();

        foreach (Sprite s in player_Sprites.playerPlanes) {
            playerPlanes.Add(s);
        }

        int indexNextPlane = playerPlanes.IndexOf(player_Sprites.playerSprite) + 1;

        if( indexNextPlane < player_Sprites.playerPlanes.Length ){
            playerNextPlane.sprite = player_Sprites.playerPlanes[indexNextPlane];

            // Se verdadeiro, atualiza o sprite do player em SO_DATA
            if (updatePlayerSprite) { 
                player_Sprites.playerSprite = player_Sprites.playerPlanes[indexNextPlane];
                setPlayerNextPlane(false);
            }

            setPlayerPlaneImage();
        }else{
            setMaxUpdateReached(planeBloqueado);
        }
            
    }

    // =====================================================
    // COMPRA DE UPGRADES
    // =====================================================

    public void spendUpgradePoints(string upgrade){
        int cost = 0;
        // bool spend = true;

        if (upgrade == "fuselagem" ){

            if( !(upgradeCosts.fuselagemSold <= upgradeCosts.maxItensSold) ){
                // seta um canvas de  maximo ja chegado
                setMaxUpdateReached(planeBloqueado);
                // seta q nao pode comprar
                return;
            }

            cost = Convert.ToInt32(fuselagemTxtCost.text);
        }
        else if (upgrade == "bombraio"){
            if ( (upgradeCosts.raioSold == 0) && (!data.playerBombEnable)){  

                cost = Convert.ToInt32(bombRaioTxtInitialCost.text);
                if( (data.upgradePoints - cost) < 0){
                    StartCoroutine(ErrorMsg()); 
                    return; 
                }
                // Habilita a bomba para o player
                data.playerBombEnable = true;

                // Desativa o painel de bomba bloqueada
                bombDesbloqueado.SetActive(false);

                // Seta a area inicial da bomba
                data.playerBombArea += data.playerInitBombArea;

                cost = upgradeCosts.raioInitialCost;
                data.upgradePoints -= cost;
                updateStatsPanel();
                setCosts();
                return;
            }
            else if ( !(upgradeCosts.raioSold < upgradeCosts.maxItensSold) ){
                // seta um canvas de  maximo ja chegado
                setMaxUpdateReached(bombBloqueado);
                // seta q nao pode comprar
                return;
            }else{
                cost = Convert.ToInt32(bombRaioTxtCost.text);
            }
        }
        else if (upgrade == "velocidade"){
            // Se 
            if( !(upgradeCosts.velocidadeSold < upgradeCosts.maxItensSold) ){
                // seta um canvas de  maximo ja chegado
                setMaxUpdateReached(velocidadeBloqueado);
                // seta q nao pode comprar
                return;
            }
            cost = Convert.ToInt32(velocidadeTxtCost.text);
        }
        else if (upgrade == "firerate"){
            if( !(upgradeCosts.firerateSold < upgradeCosts.maxItensSold) ){
                // seta um canvas de  maximo ja chegado
                setMaxUpdateReached(firerateBloqueado);
                // seta q nao pode comprar
                return;
            }
            cost = Convert.ToInt32(firerateTxtCost.text);
        }

        if( (data.upgradePoints - cost) < 0){
            StartCoroutine(ErrorMsg()); 
            return; 
        }

        data.upgradePoints -= cost;

        increasePlayerStats(upgrade);
        updateStatsPanel();
        setCosts();
    }

    private IEnumerator ErrorMsg(){
        errorMsgTxt.SetActive(true);
        yield return new WaitForSeconds(3f); 
        errorMsgTxt.SetActive(false);
    }

    private void increasePlayerStats(string upgrade){
        if (upgrade == "fuselagem"){
            // +1 vida
            // +0.5 dano
            data.playerLevel += 1;
            data.playerLife += 1;
            data.playerDamage += 1;

            upgradeCosts.fuselagemCost += upgradeCosts.fuselagemNextCost;

            upgradeCosts.fuselagemSold += 1;

            // Atualiza imagens do avião no Hangar
            // Seta o proximo avião em SO_DATA se TRUE
            updatePlayerImages(true);
        }
        if (upgrade == "bombraio"){

            data.playerBombArea += upgradeCosts.raioUpgrade;
            upgradeCosts.raioCost += upgradeCosts.raioNextCost;

            upgradeCosts.raioSold += 1;
          
        }
        if (upgrade == "velocidade"){
            data.playerSpeed += upgradeCosts.velocidadeUpgrade;
            upgradeCosts.velocidadeCost += upgradeCosts.velocidadeNextCost;

            upgradeCosts.velocidadeSold += 1;
        }
        if (upgrade == "firerate"){
            data.playerFireRate -= upgradeCosts.firerateUpgrade;
            upgradeCosts.firerateCost += upgradeCosts.firerateNextCost;   

            upgradeCosts.firerateSold += 1;       
        }
    }


    private void checkBombBought(){
        if (upgradeCosts.raioSold == 0 && !data.playerBombEnable){
            bombDesbloqueado.SetActive(true);
        }
    }

    private void setMaxUpdateReached(GameObject panel){
        panel.SetActive(true);
    }

    private void desactiveLevelMaximoPanels(){
        planeBloqueado.SetActive(false);
        bombBloqueado.SetActive(false);
        velocidadeBloqueado.SetActive(false);
        firerateBloqueado.SetActive(false);
        bombDesbloqueado.SetActive(false);
    }






    // ==========================
    // OPEN PANELS
    // ==========================

    public void openMainPanel(){
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        comoJogarPanel.SetActive(false);
    }

    public void openOptionsPanel(){
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
        comoJogarPanel.SetActive(false);

    }

    public void openComoJogarPanel(){
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        comoJogarPanel.SetActive(true);
    }

   


}
