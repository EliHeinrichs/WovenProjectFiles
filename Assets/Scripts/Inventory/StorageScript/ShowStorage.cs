using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStorage : MonoBehaviour
{
    public GameObject uiCanvas;
    public AudioClip inventorySFX;
    public GameObject indicator;
    private bool inRange;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {

            indicator.SetActive(true);
            inRange = true;

            SoundManager.Instance.PlayAudio(inventorySFX);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && GameManager.Instance.storageOpen == false && inRange == true)
        {
            uiCanvas.SetActive(true);
            GameManager.Instance.storageOpen = true;
           
            SoundManager.Instance.PlayAudio(inventorySFX);
        }
        else if (Input.GetKeyDown(KeyCode.R) && GameManager.Instance.storageOpen == true && inRange == true)
        {
            uiCanvas.SetActive(false);
            indicator.SetActive(false);
            GameManager.Instance.storageOpen = false;
            SoundManager.Instance.PlayAudio(inventorySFX);
        }


    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        inRange = false;
        uiCanvas.SetActive(false);
        indicator.SetActive(false);
        GameManager.Instance.storageOpen = false;
        SoundManager.Instance.PlayAudio(inventorySFX);
    }
 
}
