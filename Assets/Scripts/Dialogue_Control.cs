using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue_Control : MonoBehaviour
{
   [Header("Components")]
   public GameObject dialogueGO;
   public RawImage profile;
   public TMP_Text actorNameText;
   public TMP_Text speechText;
   
   [Header("Settings")]
   public float typingSpeed;
   public string[] sentences;
   public int index;
   public Dialogue myDialogue;

   // [Header("Dictionary")]
   // public string[] dictionary = {"[NOME]"};

   public void setDialogue(GameObject dial){
      // Zera as variaveis
      // this.myDialogue = null;
      // this.dialogueGO = null;
      // this.index = 0;
      // actorNameText.text = "";
      // speechText.text = "";
      

      this.dialogueGO = dial;
      this.myDialogue = dial.GetComponent<Dialogue>();
      
      // Seta as variaveis
      dialogueGO.SetActive(true);

      this.profile = myDialogue.getRawImage();
      this.profile.texture = myDialogue.getProfile();

      this.actorNameText = myDialogue.getMyActorNameText();
      this.actorNameText.text = myDialogue.getActorNameText();

      this.speechText = myDialogue.getMySpeechText();
      this.speechText.text = "";
      this.sentences = myDialogue.getSpeechText();

      StartCoroutine(TypeSentence());

   }

   IEnumerator TypeSentence(){    
      
      foreach (char letter in sentences[index].ToCharArray())
      {
         speechText.text += letter;
         yield return new WaitForSeconds(typingSpeed);
      }
   }

   public void nextSentence(){
      if(this.speechText.text == sentences[index]){
         // Ainda ha textos
         if(index < sentences.Length -1){
            index++;
            speechText.text = "";
            StartCoroutine(TypeSentence());
         }
         else{ // Lido quando acaba os textos
            if(myDialogue.getHasEvent()){
               myDialogue.activeEvent();
               gameObject.GetComponent<Hangar_Control>().dialoguePanel.SetActive(false);
            }else{
               profile.texture = null;
               speechText.text = "";
               index = 0;
               dialogueGO.SetActive(false);
               gameObject.GetComponent<Hangar_Control>().dialoguePanel.SetActive(false);
            }
         }
      }
   }
}