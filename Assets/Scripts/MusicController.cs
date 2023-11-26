using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource mainThemeStart;
    public AudioSource mainThemeLoop;


    void Start()
    {
        mainThemeStart.Play();
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
