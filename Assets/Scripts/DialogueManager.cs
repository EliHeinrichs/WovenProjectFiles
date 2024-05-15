using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueManager : MonoBehaviour
{

   public static DialogueManager Instance;
  
    public GameObject talkBox;
    public bool isTalking;
    public int talkIndex = 0;
    public TextMeshProUGUI displayText;
    public Image playerImage;
    public Image npcImage;


    public float delayText = 0.075f;
    public string fullText;
    private string currentText = "";
   
    private Coroutine textCoroutine;
    private int currentIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(string currentDialogue, Sprite npcSprite)
    {
        playerImage.sprite = GameManager.Instance.shellSlot.shellSprite;
        npcImage.sprite = npcSprite;

        // Check if coroutine is already running and stop it if it is
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
        }

        talkBox.SetActive(true);
        isTalking = true;
        fullText = currentDialogue;
        currentIndex = 0; // Reset current index
        textCoroutine = StartCoroutine(WriteText());
    }

    IEnumerator WriteText()
    {
        for (int i = currentIndex; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i + 1);
            displayText.text = currentText;

            yield return new WaitForSeconds(delayText);
        }

        // Update current index to the end of the text
        currentIndex = fullText.Length;
    }


    public void EndDialogue()
    {
      
        talkBox.SetActive(false);
        isTalking = false;
        fullText = "";
        displayText.text = "";
    }

 
 
}
