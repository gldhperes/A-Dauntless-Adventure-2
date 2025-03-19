using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class MainMenuScreen : MonoBehaviour
{
    private VisualElement screen;
    private VisualElement initialScreen;
    private VisualElement mainMenuScreen;
    private VisualElement howToPlayScreen;
    private VisualElement choosePlaneScreen;
    private Label MainTitle;


    public Player_Sprites player_Sprites;
    

    private void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
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
        AnimateInitialScreen();
    }

    private void InitScreens()
    {
        initialScreen = screen.Q<VisualElement>("InitialContainer");
        mainMenuScreen = screen.Q<VisualElement>("MainMenuContainer");
        howToPlayScreen = screen.Q<VisualElement>("HowToPlayContainer");
        choosePlaneScreen = screen.Q<VisualElement>("ChoosePlaneContainer");
        MainTitle = screen.Q<Label>("MainTitle");
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
            GameLootLoading.LoadScene(GameLootLoading.Scenes_To_Call.Fase);
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


    private void AnimateInitialScreen()
    {
        MainTitle.experimental.animation.Start(0f, 100f, 10000, (element, value) => { element.style.opacity = value; })
            .Ease(Easing.OutBounce);
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