using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] stageMusic;

    [SerializeField]
    private AudioClip stageClear;

    [SerializeField]
    private AudioClip gameover;
    
    [SerializeField]
    private AudioClip bossFight;

    [SerializeField]
    private AudioClip boss4Fight;


    // ================================
    // SETTERS ========================
    // ================================
    
    public void setStageMusic(int i){
        audioSource.clip = stageMusic[i - 1];
        audioSource.Play();
    }

    public void setBossFightMusic(){
        audioSource.clip = bossFight;
        audioSource.Play();
    }

    public void setBoss4FightMusic(){
        audioSource.clip = boss4Fight;
        audioSource.Play();
    }
    
    public void setStageClear(){
        audioSource.clip = stageClear;
        audioSource.Play();
    }

    public void setGameoverMusic(){
        audioSource.clip = gameover;
        audioSource.Play();
    }

    public void pauseAudioClip(){
        audioSource.Pause();
    }

    public void unpauseAudioClip(){
        audioSource.UnPause();
    }
}
