using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine . SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
     public AudioSource effectsSource, musicSource, walkSource;



    private void Awake ( )
    {

        if ( Instance == null )
        {
            Instance = this;
            DontDestroyOnLoad (gameObject);

        }
        else
        {
            Destroy (gameObject);
        }

     
    }



    public void PlayAudio ( AudioClip clip )
    {

        effectsSource . PlayOneShot (clip);


    }

    public void ChangeMusic ( AudioClip music )
    {
        if ( musicSource . clip == music )
        {
            return;
        }
        else
        {
            musicSource . clip = music;
            musicSource . Play ();
            musicSource . loop = true;
        }

    }

    public void WalkAudio(bool walking)
    {
        if(walking == true)
        {
            walkSource.enabled = true;
        }
        else
        {

            walkSource.enabled = false;
        }
    }

}
