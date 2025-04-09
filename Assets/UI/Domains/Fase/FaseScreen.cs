using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class FaseScreen : MonoBehaviour
{
    [SerializeField] private VisualElement screen;
    [SerializeField] private bool isPaused;
    VisualElement progressBar;
    Label lifePoints;
    Label upgradePoints;

    private VisualElement bombContainer;
    Label bombPoints;

    private VisualElement TakingOffContainer;

    private VisualElement WarningContainer;
    private VisualElement PauseMenu;

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

        this.PauseMenu = screen.Q<VisualElement>("PauseMenu");

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





    #region PAUSE_BEHAVIOUR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            // Pause or resume the game when the Escape key is pressed
            TogglePauseGame();
        }

    }

    private void TogglePauseGame()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }

        isPaused = !isPaused;
    }

    private void ResumeGame()
    {
        PauseMenu.experimental.animation.Scale(0f, 500).Ease(Easing.OutBack);
        PauseMenu.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
    }

    private void PauseGame()
    {
        PauseMenu.experimental.animation.Scale(1f, 500).Ease(Easing.OutBack);
        PauseMenu.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
    }

    #endregion PAUSE_BEHAVIOUR






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

    public IEnumerator Animate_TakingOff(float animTime, int gameLevel)
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
        yield return new WaitForSeconds(animTime);

        // Desactive TakingOffContainer
        this.TakingOffContainer.schedule.Execute(() =>
            {
                this.TakingOffContainer.style.display = DisplayStyle.None;
            })
            .StartingIn(50);


    }


    // StartCoroutine(Start_Animate_TakingOff(animTime, gameLevel));

    // IEnumerator Start_Animate_TakingOff(float timeToWait, int gameLevel)
    // {
    //     // Get the Stage Label and add the game level
    //     var stageLevel = TakingOffContainer.Q<Label>("StageLabel");
    //     stageLevel.text += gameLevel.ToString();

    //     // Activate TakingOffContainer
    //     this.TakingOffContainer.schedule.Execute(() =>
    //         {
    //             this.TakingOffContainer.style.display = DisplayStyle.Flex;
    //         })
    //         .StartingIn(50);
    //     yield return new WaitForSeconds(timeToWait);

    //     // Desactive TakingOffContainer
    //     this.TakingOffContainer.schedule.Execute(() =>
    //         {
    //             this.TakingOffContainer.style.display = DisplayStyle.None;
    //         })
    //         .StartingIn(50);


    // }



    public void Animate_Warning(float animTime, Action<float> SpawnBoss_Callback)
    {
        SpawnBoss_Callback(animTime);

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

                yield return new WaitForSeconds(animationTime / 2);

                // Desactive WarningContainer
                WarningContainer.schedule.Execute(() => { this.WarningContainer.style.display = DisplayStyle.None; })
                    .StartingIn(5);

                yield return new WaitForSeconds(animationTime / 2);
            }

        }
    }
}