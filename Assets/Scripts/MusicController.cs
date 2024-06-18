using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource mainThemeStart;
    public AudioSource mainThemeLoop;
    public AudioSource pauseTheme;
    public AudioSource deathTheme;

    bool playerIsDead;

    Object[] allAudioSourcesPlaying;

    bool gamePaused;

    void PlayerDead(Notification notificacion)
    {
        playerIsDead = true;
    }

    void GamePaused(Notification notificacion)
    {
        if (!gamePaused)
        {
            // Get all audio sources in the scene playing at the moment
            allAudioSourcesPlaying = FindObjectsByType(typeof(AudioSource), FindObjectsSortMode.None);

            gamePaused = true;
        }
        


        // Iterate through each audio source and pause it
        foreach (AudioSource audioSource in allAudioSourcesPlaying)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }


        // Pause/Death themes for when pause canvas is activated

        if (!pauseTheme.isPlaying && !playerIsDead)
        {
            pauseTheme.Play();
        }
        else if (!deathTheme.isPlaying && playerIsDead)
        {
            deathTheme.Play();
        }

        
    }

    void GameNotPaused(Notification notificacion)
    {
        if (gamePaused)
        {
            pauseTheme.Stop();

            // Iterate through each audio source and unpause it
            foreach (AudioSource audioSource in allAudioSourcesPlaying)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.UnPause();
                }
            }

            gamePaused = false;
        }
    }

    void Start()
    {
        mainThemeStart.Play();

        NotificationCenter.DefaultCenter().AddObserver(this, "GamePaused");
        NotificationCenter.DefaultCenter().AddObserver(this, "GameNotPaused");

        NotificationCenter.DefaultCenter().AddObserver(this, "PlayerDead");
    }


    void Update()
    {
        
    }

    private void FixedUpdate()
    {       
        if (!mainThemeStart.isPlaying && !mainThemeLoop.isPlaying)
        {
            mainThemeLoop.Play();
        }
    }
}
