using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneExemple : MonoBehaviour
{
    public void buttonLoadScene(string scene){
        GameLootLoading.LoadScene(scene);
    }
}
