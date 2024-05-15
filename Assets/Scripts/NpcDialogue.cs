using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcDialogue : MonoBehaviour
{
    public string[] dialogues1;
    public string[] dialogues2;
    public string[] dialogues3;
    public Sprite uiImage;


    public GameObject dialogueIndicator;
    private string currentText;

    public AudioClip talkAudio;

    private bool inRange;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            

      

            dialogueIndicator.SetActive(true);
            inRange = true;

      

     



        }
    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && inRange == true)
        {
            Debug.Log("R pressed");
            if (DialogueManager.Instance.isTalking == true)
            {
                DialogueManager.Instance.talkIndex += 1;
                Debug.Log("Continue Text");
            }
            else
            {
                DialogueManager.Instance.talkIndex = 0;
                Debug.Log("Start Text");

            }
            switch (GameManager.Instance.gameStage)
            {
                case 1:
                    if (DialogueManager.Instance.talkIndex >= dialogues1.Length)
                    {
                        dialogueIndicator.SetActive(false);
                        DialogueManager.Instance.EndDialogue();
                    }
                    currentText = dialogues1[DialogueManager.Instance.talkIndex];
                    break;

                case 2:
                    if (DialogueManager.Instance.talkIndex >= dialogues2.Length)
                    {
                        dialogueIndicator.SetActive(false);
                        DialogueManager.Instance.EndDialogue();
                    }
                    currentText = dialogues2[DialogueManager.Instance.talkIndex];
                    break;

                case 3:
                    if (DialogueManager.Instance.talkIndex >= dialogues3.Length)
                    {
                        dialogueIndicator.SetActive(false);
                        DialogueManager.Instance.EndDialogue();
                    }
                    currentText = dialogues3[DialogueManager.Instance.talkIndex];
                    break;


            }
            DialogueManager.Instance.StartDialogue(currentText,uiImage);
      
            SoundManager.Instance.PlayAudio(talkAudio);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            inRange = false;
            dialogueIndicator.SetActive(false);
            DialogueManager.Instance.EndDialogue();
        }

    }

    void OnTriggerExit2D()
    {

        inRange = false;
            dialogueIndicator.SetActive(false);
            DialogueManager.Instance.EndDialogue();
        
    }
}
