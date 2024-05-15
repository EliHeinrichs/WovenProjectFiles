using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{

    public GameObject pauseUI;
    public bool GameIsPaused;
    public AudioClip clickAudio;
    public AudioClip mainMenuAudio;
    public GameObject helpMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        pauseUI.SetActive(true);
        GameIsPaused = true;

    
        Time.timeScale = 0;
        SoundManager.Instance.PlayAudio(clickAudio);

    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        GameIsPaused = false;
  
        Time.timeScale = 1;
        SoundManager.Instance.PlayAudio(clickAudio);

    }

    public void QuitApp()
    {
        SoundManager.Instance.PlayAudio(clickAudio);
        SoundManager.Instance.ChangeMusic(mainMenuAudio);
        SceneManager.LoadScene("MainMenu");

    }

    public void HelpUI()
    {
        SoundManager.Instance.PlayAudio(clickAudio);
        if (helpMenu.activeInHierarchy)
        {
            helpMenu.SetActive(false);
        }
        else
        {
            helpMenu.SetActive(true);
        }


    }

    public void ToggleMusic()
    {
        
        SoundManager.Instance.musicSource.enabled = !SoundManager.Instance.musicSource.enabled;
    }

    public void ToggleSFX()
    {
        SoundManager.Instance.effectsSource.enabled = !SoundManager.Instance.effectsSource.enabled;
    }


    public void ToggleWalkSFX()
    {
        SoundManager.Instance.walkSource.gameObject.SetActive(!SoundManager.Instance.walkSource.gameObject.activeInHierarchy);

    }

}
