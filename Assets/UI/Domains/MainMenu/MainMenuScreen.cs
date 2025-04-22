using System;
using System.Collections;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using Task = System.Threading.Tasks.Task;

public class MainMenuScreen : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument; 
    private VisualElement screen;
    private VisualElement initialScreen;
    private VisualElement mainMenuScreen;
    private VisualElement howToPlayScreen;
    private VisualElement choosePlaneScreen;
       

    public Player_Sprites player_Sprites;


    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        screen = uiDocument.rootVisualElement;
        player_Sprites = GameObject.Find("Player Sprites").GetComponent<Player_Sprites>();

        InitScreens();
        InitButtons();
        InitChoosePlaneButtons();

        // Executa o callback depois q toda a tela for carregada
        screen.RegisterCallback<GeometryChangedEvent>(OnUIReady);
    }

    private void OnUIReady(GeometryChangedEvent evt)
    {
        // Remove o evento para evitar chamadas repetidas
        screen.UnregisterCallback<GeometryChangedEvent>(OnUIReady);

        // Agora que a UI está carregada, execute o código
        StartCoroutine(AnimateInitialScreen());
    }

    private void InitScreens()
    {
        initialScreen = screen.Q<VisualElement>("InitialContainer");
        mainMenuScreen = screen.Q<VisualElement>("MainMenuContainer");
        howToPlayScreen = screen.Q<VisualElement>("HowToPlayContainer");
        choosePlaneScreen = screen.Q<VisualElement>("ChoosePlaneContainer");
    }

    private void InitButtons()
    {
        var StartButton = screen.Q<Button>("StartButton");
        StartButton.clickable.clicked += () => ChangeScreen(mainMenuScreen);

        var NewGameButton = screen.Q<Button>("NewGameButton");
        NewGameButton.clickable.clicked += () => ChangeScreen(choosePlaneScreen);

        var HowToPlayButton = screen.Q<Button>("HowToPlayButton");
        HowToPlayButton.clickable.clicked += () => ChangeScreen(howToPlayScreen);

        var BackButton = screen.Q<Button>("BackButton");
        BackButton.clickable.clicked += () => ChangeScreen(mainMenuScreen);
    }

    private void InitChoosePlaneButtons()
    {
        void SetPlayerSprites(Sprite[] playerSprites)
        {
            setPlayerSprite(playerSprites);

            GameLootLoading gameLootLoading = FindAnyObjectByType<GameLootLoading>();
            gameLootLoading.LoadScene(Scenes_To_Call.Fase);
        }

        var RedPlaneButton = screen.Q<Button>("RedPlaneButton");
        RedPlaneButton.clickable.clicked += () => SetPlayerSprites(player_Sprites.getRedPlanes);


        var YellowPlaneButton = screen.Q<Button>("YellowPlaneButton");
        YellowPlaneButton.clickable.clicked += () => SetPlayerSprites(player_Sprites.getYellowPlanes);


        var GreenPlaneButton = screen.Q<Button>("GreenPlaneButton");
        GreenPlaneButton.clickable.clicked += () => SetPlayerSprites(player_Sprites.getGreenPlanes);


        var BluePlaneButton = screen.Q<Button>("BluePlaneButton");
        BluePlaneButton.clickable.clicked += () => SetPlayerSprites(player_Sprites.getBluePlanes);
    }

    private void setPlayerSprite(Sprite[] sprites)
    {
        player_Sprites.playerPlanes = sprites;
        player_Sprites.playerSprite = sprites[0];
    }


    private IEnumerator AnimateInitialScreen()
    {
        var mainTitleDurationsMS = 2000;
        var v2DurationMS = 1000;
        
        var MainTitle =  uiDocument.rootVisualElement.Q<Label>("MainTitle");
        var v2Label =  uiDocument.rootVisualElement.Q<Label>("v2Label");
        var clickToStart = uiDocument.rootVisualElement.Q<Label>("ClickToStart");
        var StartButton = uiDocument.rootVisualElement.Q<Button>("StartButton");
        StartButton.SetEnabled(false);
        v2Label.style.display = DisplayStyle.None;
        clickToStart.style.display = DisplayStyle.None;
        
        // Animate Main Tilte
        MainTitle.experimental.animation.Start(new Vector2(0, 0), new Vector2(1, 1), mainTitleDurationsMS,
            (element, value) => { element.style.scale = new StyleScale(value); }).Ease(Easing.OutElastic);

        // Wait for the animation is over
        yield return new WaitForSeconds(.5f);
        
        // Set V2Label to flex and animate it
        v2Label.style.display = DisplayStyle.Flex;
        v2Label.experimental.animation.Start(new Vector2(5, 5), new Vector2(1, 1), v2DurationMS,
            (element, value) => { element.style.scale = new StyleScale(value); }).Ease(Easing.OutBounce);
        
        // Wait for the animation is over
        yield return new WaitForSeconds(1f);
        
        clickToStart.style.display = DisplayStyle.Flex;
        StartButton.SetEnabled(true);
    }

  

    private void ChangeScreen(VisualElement screen_to_open)
    {
        initialScreen.style.display = DisplayStyle.None;

        if (screen_to_open == mainMenuScreen)
        {
            mainMenuScreen.style.display = DisplayStyle.Flex;
            howToPlayScreen.style.display = DisplayStyle.None;
            choosePlaneScreen.style.display = DisplayStyle.None;
        }
        else if (screen_to_open == howToPlayScreen)
        {
            mainMenuScreen.style.display = DisplayStyle.None;
            howToPlayScreen.style.display = DisplayStyle.Flex;
            choosePlaneScreen.style.display = DisplayStyle.None;
        }
        else
        {
            mainMenuScreen.style.display = DisplayStyle.None;
            howToPlayScreen.style.display = DisplayStyle.None;
            choosePlaneScreen.style.display = DisplayStyle.Flex;
        }
    }
}