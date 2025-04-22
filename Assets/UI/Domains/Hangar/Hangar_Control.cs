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
            // Atualiza a Sprite do Proximo Plane na classe Player_Sprites
            if (upgradeType == UpgradeType.NextPlane)
            {
                player_Sprites.SetPlayerNextPlane();
            }

            // Atualiza o Status do jogador
            HangarScreen hs = GetComponent<HangarScreen>();
            hs.UpdatePlayerStatusBar(upgradeType);

            // Atualiaza custo do próximo upgrade 
            // upgradeCostsDic[upgradeType] = upgradeCosts.GetNextUpgrade(upgradeType);


            OnUpgradePurchased?.Invoke(upgradeType);
        }
    }


    // Pega a chave do proximo custo de upgrade
    public float GetNextUpgradeCost(UpgradeType upgradeType) => upgradeCostsDic[upgradeType];
}