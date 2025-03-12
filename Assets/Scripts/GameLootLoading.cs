using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLootLoading : MonoBehaviour
{
    public TMP_Text percentText;
    public Image foregroundImage;
    // public Scene scene;
    void Start()
    {   
        // Se nunca carregou uma scene carrega a primeira, no caso MainMenu
        string sceneName = PlayerPrefs.GetString("SCENE_TO_LOAD", "MainMenu");
        PlayerPrefs.SetString("SCENE_TO_LOAD", "MainMenu");
        PlayerPrefs.Save();
        percentText.text = "0%";
        foregroundImage.fillAmount = 0;
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public static void LoadScene(string sceneName){
        PlayerPrefs.SetString("SCENE_TO_LOAD", sceneName);
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadSceneAsync(string sceneName){

        System.GC.Collect();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        // asyncLoad.allowSceneActivation = false;

        while(!asyncLoad.isDone){
            percentText.text = (asyncLoad.progress * 100).ToString("N3") + "%";
            foregroundImage.fillAmount = asyncLoad.progress;
            yield return new WaitForSeconds(0);
            // yield return;
        }

        // System.GC.Collect();
        // asyncLoad.allowSceneActivation = true;
    }
}
