using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Hangar_Control : MonoBehaviour
{
    public Action<UpgradeType> OnUpgradePurchased;
    
    [Header("SO_DATA")] public SO_Data data;
    public UpgradeCosts upgradeCosts;
    public Player_Sprites player_Sprites;

    private Dictionary<UpgradeType, int> upgradeCostsDic;

    void Start()
    {
        data = GameObject.Find("SO_DATA").GetComponent<SO_Data>();
        upgradeCosts = GameObject.Find("UpgradeCosts").GetComponent<UpgradeCosts>();
        player_Sprites = GameObject.Find("Player Sprites").GetComponent<Player_Sprites>();

        upgradeCostsDic = new()
        {
            { UpgradeType.NextPlane, upgradeCosts.fuselagemCost },
            { UpgradeType.Bomb, upgradeCosts.raioCost },
            { UpgradeType.Speed, upgradeCosts.speedCost },
            { UpgradeType.FireRate, upgradeCosts.firerateCost }
        };
    }

    public void UpgradePlane()
    {
        // APLICAR MUDANÇAS EM HANGARSCREEN


        // Se nao tiver vendido nada entao vende
        // Senao, é o level maximo
        if (!upgradeCosts.fuselagemSold)
        {
            player_Sprites.GetNextPlayerPlane();

            // Chamar isso em SO_Data
            // data.playerLevel += 1;
            // data.playerLife += 1;
            // data.playerDamage += 1;

            // Atualizar texto na status bar para o nivel atual
        }
       
    }

    public void UpgradeBomb()
    {
        // APLICAR MUDANÇAS EM HANGARSCREEN


        // Checa se é a primeira vez que esta liberando a bomba
        if (!data.playerBombEnable)
        {
            data.playerBombEnable = true;

            // Liberar cadeado em HangarScreen
        }

        data.playerBombArea += .75f;
    }

    public void UpgradeSpeed()
    {
        // APLICAR MUDANÇAS EM HANGARSCREEN

        data.playerSpeed += 2;
    }

    public void UpgradeFirerate()
    {
        // APLICAR MUDANÇAS EM HANGARSCREEN

        data.playerFireRate += .1f;
    }

    public void PlayNextStage()
    {
        GameLootLoading gameLootLoading = FindAnyObjectByType<GameLootLoading>();
        gameLootLoading.LoadScene(Scenes_To_Call.Fase);
    }

    public void TryBuyUpgrade(UpgradeType upgradeType)
    {
        // Pega o custo atual do Upgrade
        int cost = upgradeCostsDic[upgradeType];
        bool success = data.TryPurchase(upgradeType, cost);

        if (success)
        {
            // Atualiza o Status do jogador
            HangarScreen hs = GetComponent<HangarScreen>();
            hs.UpdatePlayerStatusBar(upgradeType); 
            
            // Atualiaza custo do próximo upgrade 
            upgradeCostsDic[upgradeType] = upgradeCosts.GetNextUpgrade(upgradeType);
            
            // Atualiza a Sprite do Proximo Plane
            player_Sprites.SetPlayerNextPlane();
            
            OnUpgradePurchased?.Invoke(upgradeType);
        }
    }


    // Pega a chave do proximo custo de upgrade
    public float GetNextUpgradeCost(UpgradeType upgradeType) => upgradeCostsDic[upgradeType];
}