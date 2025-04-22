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
    public static GameLootLoading Instance;
    void Awake()
    {
        // Se já existe uma instância diferente, destrói esse novo
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Define como instância única
        Instance = this;
    }

    private void Start()
    {
        // Impede que seja destruído ao trocar de cena
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(Scenes_To_Call.GatesLoadingScene, LoadSceneMode.Additive);
        StartCoroutine(LoadSceneAsync(sceneName));

    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(3f);

        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log(asyncLoad.progress);
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;

        // Animação de abrir os portões
        var GateLoadingScreen = FindAnyObjectByType<GateLoadingScreen>();
        if (GateLoadingScreen != null)
        {
            GateLoadingScreen.OpenGate();
        }
        else
        {
            Debug.LogError("GateLoadingScreen not found in the scene.");
        }
    }

    public void LoadGameOverScreen()
    {
        SceneManager.LoadSceneAsync(Scenes_To_Call.Gameover);
    }

    public void RestartLevel()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Scenes_To_Call.Fase);
        asyncLoad.allowSceneActivation = true;
    }

}

public static class Scenes_To_Call
{
    public const string Creditos = "Creditos";
    public const string Fase = "Fase";
    public const string Hangar = "Hangar";
    public const string Gameover = "GameOver";
    public const string GatesLoadingScene = "GatesLoadingScene";

}