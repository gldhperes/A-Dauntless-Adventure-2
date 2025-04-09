using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLootLoading : MonoBehaviour
{
   
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void LoadScene(string scene_name)
    {
        StartCoroutine(LoadSceneAsync(scene_name));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
     
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        

        // Nao ativa a cena quando estiver pronta, pois iremos fazer requisições
        asyncLoad.allowSceneActivation = true;
        
        // Destroy(this.gameObject);

        yield return null;
    }
    
}

public static class Scenes_To_Call
{
    public const string Creditos = "Creditos";
    public const string Fase = "Fase";
    public const string Hangar = "Hangar";

    public const string LoadingScene = "LoadingScene";

    // public const string Gameover = "GameOver";
    public const string MainMenu = "MainMenu";
}