using UnityEngine;
using UnityEngine.UIElements;

public class HangarScreen : MonoBehaviour
{
    [Header("Components")] [SerializeField]
    private UIDocument uiDocument;

    [SerializeField] private Hangar_Control hangarControl;
    private VisualElement screen;


    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        screen = uiDocument.rootVisualElement;
        hangarControl = GetComponent<Hangar_Control>();
        InitScreen();
    }

    private void InitScreen()
    {
        int playerLvl = hangarControl.data.playerLevel;

        // LEVEL CONTAINER
        var planeImg = screen.Q<VisualElement>("PlaneImg");
        planeImg.style.backgroundImage = new StyleBackground(hangarControl.player_Sprites.playerSprite);

        var playerLevel = screen.Q<Label>("PlayerLevel");
        playerLevel.text = "Lvl: " + playerLvl;

        var upgradeValue = screen.Q<Label>("UpgradeValue");
        upgradeValue.text = hangarControl.data.upgradePoints.ToString();

        var lifeValue = screen.Q<Label>("LifeValue");
        lifeValue.text = hangarControl.data.playerLife.ToString();

        // STATUS CONTAINER
        var dmgValue = screen.Q<Label>("DamageValue");
        dmgValue.text = hangarControl.data.playerDamage.ToString();

        var fireRate = screen.Q<Label>("FirerateValue");
        fireRate.text = hangarControl.data.playerFireRate.ToString();

        var speed = screen.Q<Label>("SpeedValue");
        speed.text = hangarControl.data.playerSpeed.ToString();

        var bombValue = screen.Q<Label>("BombValue");
        bombValue.text = hangarControl.data.playerBombArea.ToString();


        // UPGRADE SHOP CONTAINER

        //NEXT PLANE UPGRADE
        var nextPlaneImg = screen.Q<VisualElement>("NextPlaneImg");

        if (playerLvl < hangarControl.player_Sprites.playerPlanes.Length)
        {
            var nextPlaneValue = screen.Q<Label>("NextPlaneValue");
            nextPlaneValue.text = hangarControl.upgradeCosts.fuselagemCost.ToString();

            nextPlaneImg.style.backgroundImage =
                new StyleBackground(hangarControl.player_Sprites.playerPlanes[playerLvl]);
        }
        else
        {
            // BLOCK playerLvl
        }

        // BOMB UPGRADE
        var BombUpgradeValue = screen.Q<Label>("BombUpgradeValue");
        BombUpgradeValue.text = hangarControl.upgradeCosts.raioCost.ToString();

        // SPEED UPGRADE
        var SpeedUpgradeValue = screen.Q<Label>("SpeedUpgradeValue");
        SpeedUpgradeValue.text = hangarControl.upgradeCosts.speedCost.ToString();

        // FIRERATE UPGRADE
        var FirerateUpgradeValue = screen.Q<Label>("FirerateUpgradeValue");
        FirerateUpgradeValue.text = hangarControl.upgradeCosts.firerateCost.ToString();
    }
}