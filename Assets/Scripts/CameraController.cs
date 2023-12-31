using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private float angle;
    private float camX;
    private float camY;
    public float initialCamX = 0;
    public float initialCamY = 0;
    public float camShakeMultiplier = 0.1f;
    private GameObject player;

    public static bool pause = false;
    public Volume globalVolume;
    private DepthOfField blurEffect;

    public GameObject pauseCanvas;

    public float distanceIncrementRatio = 0.05f;
    private float rawDistance;
    public static int distance;

    void CameraShake(bool activate)
    {
        angle = 360 * UnityEngine.Random.value;
        camX = camShakeMultiplier * Mathf.Cos(angle);
        camY = camShakeMultiplier * Mathf.Sin(angle);

        //if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("stand"))
        //{
            if (activate == true)
            {
                transform.position = new Vector3(
                    transform.position.x + camX,
                    transform.position.y + camY,
                    -10);
            }
            else
            {
                transform.position = new Vector3(
                    initialCamX,
                    initialCamY,
                    -10);
            }
        //}
    }

    public void MenuHandler()
    {
        if (!player.GetComponent<Animator>().GetBool("death"))
        {
            CameraShake(false);

            if (pause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        pause = true;
        blurEffect.active = true;
        pauseCanvas.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        pause = false;
        blurEffect.active = false;
        pauseCanvas.transform.GetChild(0).gameObject.SetActive(false);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (globalVolume.profile.TryGet(out DepthOfField tmp))
        {
            blurEffect = tmp;
        }
        blurEffect.gaussianMaxRadius = new ClampedFloatParameter(1,0,1);
    }

   
    void Update()
    {
        if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hurt") && !pause)
        {
            CameraShake(true);
        }
        else
        {
            CameraShake(false);
        }

        // Distance measurements

        if (!pause && !player.GetComponent<Animator>().GetBool("death"))
        {
            rawDistance += distanceIncrementRatio * player.GetComponent<Animator>().speed;
        }
        

        distance = Mathf.RoundToInt(rawDistance);

        pauseCanvas.transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>().text = distance + " m";

        // For game pausing on PC

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CameraShake(false);

            if (pause)
            {
                ResumeGame();            
            }
            else
            {
                PauseGame();
            }
        }

        // to reset game testing

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
