using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.Unicode;



public class DungeonExit : MonoBehaviour
{
    public float timer = 10f;
    public string sceneName;
    public GameObject popupObj;
    private bool popup;
    public bool pit;
    public AudioClip exitSFX;
    public AudioClip musicChange;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && popup == false)
        {

            popupObj.SetActive(true);
            popup = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && popup == true)
        {
            popupObj.SetActive(false);
            popup = false;
        }
    }



    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;


        if (popup == true && Input.GetKeyDown(KeyCode.R))
        {

            if (pit == true)
            {
                int drop = Random.Range(2, 5);
                GameManager.Instance.level += drop;
                if (drop >= GameManager.Instance.currentHp)
                {
                    GameManager.Instance.currentHp = 1;
                }
                else
                {

                    GameManager.Instance.currentHp -= drop;
                }
                
            }
            SceneManager.LoadScene(sceneName);
            SoundManager.Instance.ChangeMusic(musicChange);
                                     // Reload this scene
            GameManager.Instance.level += 1;
            SoundManager.Instance.PlayAudio(exitSFX);  //let me cook
            
        }




    }
}