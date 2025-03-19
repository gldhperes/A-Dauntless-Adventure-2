using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FaseScreen : MonoBehaviour
{
    [SerializeField] private VisualElement screen;
    VisualElement progressBar;
    Label lifePoints;
    Label upgradePoints;

    private VisualElement bombContainer;
    Label bombPoints;

    public void InitScreen(SO_Data player_data, Player_Behavior player_Behavior)
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        this.screen = uiDocument.rootVisualElement;

        this.progressBar = screen.Query<VisualElement>("ProgressBar");

        this.lifePoints = screen.Q<Label>("LifePoints");
        this.lifePoints.text = player_data.playerLife.ToString();

        this.upgradePoints = screen.Q<Label>("UpgradePoints");
        this.upgradePoints.text = player_data.upgradePoints.ToString();

        this.bombContainer = screen.Q<VisualElement>("BombStatus");

        if (player_data.playerBombEnable)
        {
            bombPoints.style.display = DisplayStyle.Flex;
            this.bombPoints = screen.Q<Label>("BombPoints");
        }
        else
        {
            bombContainer.style.display = DisplayStyle.None;
        }
    }

    public void UpdatePlayerLife(int life)
    {
        this.lifePoints.text = life.ToString();
    }

    public void UpdateUpgradePoints(int upgrade)
    {
        this.upgradePoints.text = upgrade.ToString();
    }

    public void UpdateProgressBar(float progress)
    {
        this.progressBar.style.height = new StyleLength(new Length(progress * 100, LengthUnit.Percent));
    }
}