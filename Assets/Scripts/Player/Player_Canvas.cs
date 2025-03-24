using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Canvas : MonoBehaviour
{
    [SerializeField] private TMP_Text faseText;

    [Header("Player Settings")]
    public Game_Events game_Events;
    public Player_Behavior player_Behavior;
  
    [Header("Pause Settings")]
    public bool isPause = false;
     

    void Start(){
        Unpause();
    }

    public void setPlayerBehaviour(Player_Behavior pb){
        this.player_Behavior = pb;
   
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
        // gameOverPanel.SetActive(true);
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
        // activePauseMenu();
    }

    public void Unpause(){
        Time.timeScale = 1f;
        game_Events.getAudioSettings().unpauseAudioClip();
        // activeStatusMenu();  // desactive Pause Menu
    }

   

    // BUTTONS FUNCTIONS ======================================
    public void restartLevel(){
        GameLootLoading.LoadScene(GameLootLoading.Scenes_To_Call.Fase);
    }

    public void quitLevel(){
        GameLootLoading.LoadScene(GameLootLoading.Scenes_To_Call.Hangar);
    }        
}
