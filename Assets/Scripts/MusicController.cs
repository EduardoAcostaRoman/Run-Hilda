using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip mainThemeStart;
    public AudioClip mainThemeLoop;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = mainThemeStart;
        audioSource.Play();
    }

    
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = mainThemeLoop;
            audioSource.Play();
        }
    }
}
