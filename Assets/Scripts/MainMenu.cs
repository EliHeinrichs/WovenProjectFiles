using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject creditsUI;

    public AudioClip clickAudio;


    public AudioClip townAudio;



    public void QuitApp()
    {
        SoundManager.Instance.PlayAudio(clickAudio);
        Application.Quit();

    }
    public void PlayGame()
    {
        SoundManager.Instance.PlayAudio(clickAudio);
        SoundManager.Instance.ChangeMusic(townAudio);
        SceneManager.LoadScene("Town");


    }


    public void CreditsUI()
    {
        SoundManager.Instance.PlayAudio(clickAudio);
        if (creditsUI.activeInHierarchy)
        {
            creditsUI.SetActive(false);
        }
        else
        {
            creditsUI.SetActive(true);
        }


    }
}
