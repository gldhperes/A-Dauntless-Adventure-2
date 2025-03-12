using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Header ("Components")]
    public RawImage myRawImage;
    public TMP_Text myActorNameText;
    public TMP_Text mySpeechText;
    public Texture profile;
    public string[] speechText;
    public string actorName;
    public bool hasEvent;
    public Dialogue_Event myEvent;

    public Texture getProfile(){
        return this.profile;
    }

    public RawImage getRawImage(){
        return this.myRawImage;
    }

    public string getActorNameText(){
        return this.actorName;
    }
    
    public TMP_Text getMyActorNameText(){
        return this.myActorNameText;
    }

    public string[] getSpeechText(){
        return this.speechText;
    }

    public TMP_Text getMySpeechText(){
        return this.mySpeechText;
    }

    public bool getHasEvent(){
        return this.hasEvent;
    }

    public void activeEvent(){
        myEvent.activeEvent();
    }

}
