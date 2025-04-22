using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HangarScreen : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private UIDocument uiDocument;

    private VisualElement screen;
    private VisualElement lockedItem;
    [SerializeField] private VisualTreeAsset lockedItemTemplate;

    [SerializeField] private Hangar_Control hangarControl;

    private Dictionary<UpgradeType, Action> statusDictionary = new();

    private Dictionary<UpgradeType, (Button button, Label costLabel)> buttons = new()
    {
        { UpgradeType.NextPlane, (null, null) },
        { UpgradeType.Bomb, (null, null) },
        { UpgradeType.Speed, (null, null) },
        { UpgradeType.FireRate, (null, null) }
    };

    #region START_FUNCTIONS

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        screen = uiDocument.rootVisualElement;
        hangarControl = GetComponent<Hangar_Control>();
        InitScreenTexts();
        InitButtonsListeners();
        InitStatusDict();

    }

    private void InitScreenTexts()
    {
        int playerLvl = hangarControl.data.playerLevel;

        // LEVEL CONTAINER
        var planeImg = screen.Q<VisualElement>("PlaneImg");
        planeImg.style.backgroundImage = new StyleBackground(hangarControl.player_Sprites.playerSprite);


        var playerLevel = screen.Q<Label>("PlayerLevel");
        playerLevel.text = "Lvl: " + playerLvl;

        // Atualiza o valor dos pontos de Upgrade
        UpdatePlayerUpgradePointsUI(hangarControl.data.upgradePoints);

        var lifeValue = screen.Q<Label>("LifeValue");
        lifeValue.text = hangarControl.data.playerLife.ToString();

        // STATUS CONTAINER
        var dmgValue = screen.Q<Label>("DamageValue");
        dmgValue.text = hangarControl.data.playerDamage.ToString();

        var fireRateValue = screen.Q<Label>("FireRateValue");
        fireRateValue.text = hangarControl.data.playerFireRate.ToString();

        var speedValue = screen.Q<Label>("SpeedValue");
        speedValue.text = hangarControl.data.playerSpeed.ToString();

        var bombValue = screen.Q<Label>("BombValue");
        bombValue.text = hangarControl.data.playerBombArea.ToString();


        //NEXT PLANE UPGRADE
        var nextPlaneImg = screen.Q<VisualElement>("NextPlaneImg");

        if (playerLvl < hangarControl.player_Sprites.playerPlanes.Length)
        {
            var nextPlaneValue = screen.Q<Label>("NextPlaneUpgradeValue");
            nextPlaneValue.text = hangarControl.upgradeCosts.fuselagemCost.ToString();

            nextPlaneImg.style.backgroundImage =
                new StyleBackground(hangarControl.player_Sprites.playerPlanes[playerLvl]);
        }
        else
        {
            // BLOCK playerLvl
            AddLockedItemToContainer("NextPlaneUpgradeButton");

        }

        // BOMB UPGRADE
        var BombUpgradeValue = screen.Q<Label>("BombUpgradeValue");
        BombUpgradeValue.text = hangarControl.upgradeCosts.raioCost.ToString();

        // SPEED UPGRADE
        var SpeedUpgradeValue = screen.Q<Label>("SpeedUpgradeValue");
        SpeedUpgradeValue.text = hangarControl.upgradeCosts.speedCost.ToString();

        // FIRERATE UPGRADE
        var FirerateUpgradeValue = screen.Q<Label>("FireRateUpgradeValue");
        FirerateUpgradeValue.text = hangarControl.upgradeCosts.firerateCost.ToString();
    }

    private void InitButtonsListeners()
    {
        // Inscrever-se nos eventos
        hangarControl.data.OnMoneyChanged += UpdatePlayerUpgradePointsUI;
        hangarControl.OnUpgradePurchased += UpdateUpgradeValueUI;

        var playButton = screen.Q<Button>("PlayButton");
        playButton.clickable.clicked += hangarControl.PlayNextStage;


        foreach (UpgradeType Type in UpgradeNames.upgradesDict.Keys)
        {
            string upgrade = UpgradeNames.upgradesDict[Type];

            Button btn = screen.Q<Button>($"{upgrade}UpgradeButton");

            Label costLbl = screen.Q<Label>($"{upgrade}UpgradeValue");

            btn.clickable.clicked += () => { BuyUpgrade(Type); };

            buttons[Type] = (btn, costLbl);
        }
    }

    private void InitStatusDict()
    {
        statusDictionary = new Dictionary<UpgradeType, Action>
        {
            { UpgradeType.NextPlane, UpdateNextPlane },
            { UpgradeType.Bomb, UpdateBomb },
            { UpgradeType.Speed, UpdateSpeed },
            { UpgradeType.FireRate, UpdadetFireRate },
        };
    }


    public void AddLockedItemToContainer(string containerName)
    {
        lockedItem = lockedItemTemplate.CloneTree();
        lockedItem.style.position = Position.Absolute;
        lockedItem.style.width = new StyleLength(Length.Percent(100));
        lockedItem.style.height = new StyleLength(Length.Percent(100));



        // Add it to the BombContainer
        var Container = screen.Q<VisualElement>(containerName);
        Container.SetEnabled(false);
        Container.Add(lockedItem);
        
        
    }

    #endregion START_FUNCTIONS


    #region UPDATE_STATUS
    private void BuyUpgrade(UpgradeType upgradeName)
    {
        hangarControl.TryBuyUpgrade(upgradeName);
    }

    public void UpdatePlayerStatusBar(UpgradeType upgradetype)
    {
        statusDictionary[upgradetype]?.Invoke();
    }

    private void UpdateNextPlane()
    {
        var dmgValue = screen.Q<Label>("DamageValue");
        dmgValue.text = hangarControl.data.playerDamage.ToString();

        var playerLevel = screen.Q<Label>("PlayerLevel");
        playerLevel.text = "Lvl: " + hangarControl.data.playerLevel.ToString();

        var maxLifeValue = screen.Q<Label>("LifeValue");
        maxLifeValue.text = hangarControl.data.playerLife.ToString();

        var planeImg = screen.Q<VisualElement>("PlaneImg");
        Sprite currentPlane = hangarControl.player_Sprites.playerSprite;
        planeImg.style.backgroundImage = new StyleBackground(currentPlane);

        var nextPlaneImg = screen.Q<VisualElement>("NextPlaneImg");
        Sprite nextPlane = hangarControl.player_Sprites.GetNextPlayerPlane();
        nextPlaneImg.style.backgroundImage = new StyleBackground(nextPlane);
    }

    private void UpdateBomb()
    {
        var bombValue = screen.Q<Label>("BombValue");
        bombValue.text = hangarControl.data.playerBombArea.ToString();
    }

    private void UpdateSpeed()
    {
        var speedValue = screen.Q<Label>("SpeedValue");
        speedValue.text = hangarControl.data.playerSpeed.ToString();
    }

    private void UpdadetFireRate()
    {
        var firerateValue = screen.Q<Label>("FireRateValue");
        firerateValue.text = hangarControl.data.playerFireRate.ToString();
    }

    #endregion UPDATE_STATUS



    #region ACTIONS 
    private void UpdatePlayerUpgradePointsUI(int upgradePoints)
    {
        var upgradeValue = screen.Q<Label>("UpgradeValue");
        upgradeValue.text = upgradePoints.ToString();
    }

    private void UpdateUpgradeValueUI(UpgradeType upgradeType)
    {
        buttons[upgradeType].costLabel.text = $"{hangarControl.GetNextUpgradeCost(upgradeType)}";
    }
    #endregion ACTIONS


 
}