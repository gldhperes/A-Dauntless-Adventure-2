using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Canvas : MonoBehaviour
{
    [Header ("Panels")]
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    // public GameObject isMobilePanel;
    public GameObject comoJogarMenu;
    public GameObject decolandoPanel;
    // public GameObject playerStats;
    public GameObject exitButton;
    public GameObject gameOverPanel;

    [SerializeField] private TMP_Text faseText;

    [Header("Player Settings")]
    public Game_Events game_Events;
    public Player_Behavior player_Behavior;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI lifeText;
    public Image shotImage;
    public Image bombImage;
    public Image shieldImage;

    [Header("Pause Settings")]
    public bool isPause = false;
     

    void Start(){
        if(game_Events.getGameLevel() <= 1){ exitButton.SetActive(false); }
 
        desactiveAllPanels();
        Unpause();
    }

    public void setPlayerBehaviour(Player_Behavior pb){
        this.player_Behavior = pb;
        setUpgradeAndLifeTexts();
    }

    public void setUpgradeAndLifeTexts(){
        upgradeText.text = player_Behavior.getUpgradePoints().ToString();
        lifeText.text = player_Behavior.getLifePoints().ToString();
    }

    private void desactiveAllPanels(){
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        comoJogarMenu.SetActive(false);
        decolandoPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    void Update()
    { pauseGame(); } 

    public void pauseGame(){
        // if( Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) ){
        if( Input.GetKeyDown(KeyCode.P) ){
            togglePause();
        }
    }
    
    // ========================================================
    // GAME OVER ==============================================
    // ========================================================
    public void gameOver(){
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }



    // ========================================================
    // PAUSE GAME =============================================
    // ========================================================
    public void togglePause(){
        isPause = !isPause;

        if(isPause){
            Pause();
        }else{
            Unpause();
        }
    }

    private void Pause(){
        Time.timeScale = 0;
        game_Events.getAudioSettings().pauseAudioClip();
        activePauseMenu();
    }

    public void Unpause(){
        Time.timeScale = 1f;
        game_Events.getAudioSettings().unpauseAudioClip();
        activeStatusMenu();  // desactive Pause Menu
    }


    public void updateUpgradeText(){
        upgradeText.text = player_Behavior.getUpgradePoints().ToString();
    }

    public void updateLifeText(){
        lifeText.text = player_Behavior.getLifePoints().ToString();
    }

    // ==========================================================
    // GETTERS ==================================================
    // ==========================================================
    public TextMeshProUGUI getUpgradeText(){
        return this.upgradeText;
    }

    public TextMeshProUGUI getLifeText(){
        return this.lifeText;
    }

    public Image getShieldImage(){
        return this.shieldImage;
    }

    public Image getShotImage(){
        return this.shotImage;
    }

    public Image getBombImage(){
        return this.bombImage;
    }

    public void setFaseText(string lvl){
        faseText.text += " " + lvl;
    }


    // BUTTONS FUNCTIONS ======================================
    public void activePauseMenu(){
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        comoJogarMenu.SetActive(false);
        // playerStats.SetActive(false);
        decolandoPanel.SetActive(false);
        // isMobilePanel.SetActive(false);
    }

    public void activeStatusMenu(){
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        comoJogarMenu.SetActive(false);
        // playerStats.SetActive(true);

        // isMobilePanel.SetActive(true);
        
        decolandoPanel.SetActive(false);
    }

    public void activeOptionsMenu(){
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        comoJogarMenu.SetActive(false);
        // playerStats.SetActive(false);
    }

    public void activeComoJogarMenu(){
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        comoJogarMenu.SetActive(true);
        // playerStats.SetActive(false);
    }

    public void restartLevel(){
        GameLootLoading.LoadScene("Fase");
    }

    public void quitLevel(){
        GameLootLoading.LoadScene("Hangar");
    }        
}
