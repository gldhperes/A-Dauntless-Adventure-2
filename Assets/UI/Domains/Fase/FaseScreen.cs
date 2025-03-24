using System;
using System.Collections;
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

    private VisualElement TakingOffContainer;

    private VisualElement WarningContainer;

    public void InitScreen(SO_Data player_data, Player_Behavior player_Behavior)
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        this.screen = uiDocument.rootVisualElement;

        this.TakingOffContainer = screen.Q<VisualElement>("TakingOffContainer");
        this.WarningContainer = screen.Q<VisualElement>("WarningContainer");

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

    public void Animate_TakingOff(float animTime, int gameLevel, Action Continue_Before_Callback)
    {
        StartCoroutine(Start_Animate_TakingOff(animTime, gameLevel));
        
        IEnumerator Start_Animate_TakingOff(float timeToWait, int gameLevel)
        {
            // Get the Stage Label and add the game level
            var stageLevel = TakingOffContainer.Q<Label>("StageLabel");
            stageLevel.text += gameLevel.ToString();

            // Activate TakingOffContainer
            this.TakingOffContainer.schedule.Execute(() =>
                {
                    this.TakingOffContainer.style.display = DisplayStyle.Flex;
                })
                .StartingIn(50);
            yield return new WaitForSeconds(timeToWait);
            
            // Desactive TakingOffContainer
            this.TakingOffContainer.schedule.Execute(() =>
                {
                    this.TakingOffContainer.style.display = DisplayStyle.None;
                })
                .StartingIn(50);

            Continue_Before_Callback();
        }
    }
    
  

    public void Animate_Warning(float animTime, Action SpawnBoss_Callback)
    {
        StartCoroutine(Start_Animate_Warning(animTime));

        IEnumerator Start_Animate_Warning(float animTime)
        {
            // Refer to how many times the animation occurs
            int animLoopTimes = 3;
            float animationTime = animTime / animLoopTimes;
            
            for (int i = 0; i < animLoopTimes; i++)
            {
                // Activate WarningContainer
                WarningContainer.schedule.Execute(() => { this.WarningContainer.style.display = DisplayStyle.Flex; })
                    .StartingIn(5);
                
                yield return new WaitForSeconds(animationTime/2);

                // Desactive WarningContainer
                WarningContainer.schedule.Execute(() => { this.WarningContainer.style.display = DisplayStyle.None; })
                    .StartingIn(5);
                
                yield return new WaitForSeconds(animationTime/2);
            }

            SpawnBoss_Callback();
        }
    }
}