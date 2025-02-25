using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource mainThemeStart;
    public AudioSource mainThemeLoop;
    public AudioSource pauseTheme;
    public AudioSource deathTheme;

    public AudioSource demonBossTheme;

    bool playerIsDead;

    Object[] allAudioSourcesPlaying;

    AudioSource presentAudioPlaying;

    bool gamePaused;

    bool bossSpawned;
    bool bossIsDead;

    GameObject boss;
    AudioSource bossThemeToPlay;

    bool bossThemeStart;

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
            if (audioSource && audioSource.isPlaying)
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
                if (audioSource && !audioSource.isPlaying)
                {
                    audioSource.UnPause();
                }
            }

            gamePaused = false;
        }
    }

    void BossSpawned(Notification notificacion)
    {
        bossSpawned = true;
    }

    void Start()
    {
        mainThemeStart.Play();

        NotificationCenter.DefaultCenter().AddObserver(this, "GamePaused");
        NotificationCenter.DefaultCenter().AddObserver(this, "GameNotPaused");

        NotificationCenter.DefaultCenter().AddObserver(this, "PlayerDead");

        NotificationCenter.DefaultCenter().AddObserver(this, "BossSpawned");
    }


    void Update()
    {

        // checks which boss is currently spawned
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in Enemies)
        {
            if (enemy.name.Contains("Demon"))
            {
                boss = enemy;
                bossThemeToPlay = demonBossTheme;
            }
        }

        // starts the main boss theme if there is one 
        if (boss && boss.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("stand") && !bossThemeStart)
        {
            bossThemeToPlay.Play();
            bossThemeStart = true;
        }


        // stops the main boss theme when dying
        if (boss && boss.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hurt") && bossThemeStart)
        {
            bossIsDead = true;
            bossThemeStart = false;
        }
    }

    private void FixedUpdate()
    {
        // to start the loop of the main music after the song start sequence
        if (!mainThemeStart.isPlaying && !mainThemeLoop.isPlaying)
        {
            mainThemeLoop.Play();
        }
        

        // to check which music is playing without messing up with the other audio sources playing
        presentAudioPlaying = mainThemeStart.isPlaying ? mainThemeStart : presentAudioPlaying;
        presentAudioPlaying = mainThemeLoop.isPlaying ? mainThemeLoop : presentAudioPlaying;


        // maximum volume reachable for every song
        mainThemeStart.volume = Mathf.Clamp(mainThemeStart.volume, 0, 0.25f);
        mainThemeLoop.volume = Mathf.Clamp(mainThemeLoop.volume, 0, 0.25f);
        demonBossTheme.volume = Mathf.Clamp(demonBossTheme.volume, 0, 0.2f);


        // sequence to lower the volume gradually of the main music and starting the boss theme
        if (bossSpawned)
        {
            if (presentAudioPlaying.volume > 0)
            {
                presentAudioPlaying.volume -= 0.01f;
            }

            if (bossThemeStart)
            {
                bossThemeToPlay.volume += 0.01f;
            }

            if (bossIsDead)
            {
                if (bossThemeToPlay.volume > 0)
                {
                    bossThemeToPlay.volume -= 0.01f;
                }
                else
                {
                    bossIsDead = false;
                    bossSpawned = false;
                }
            }
        }
        else
        {
            presentAudioPlaying.volume += 0.01f;
        }
    }
}
