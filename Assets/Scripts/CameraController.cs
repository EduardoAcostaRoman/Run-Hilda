using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private float angle;
    private float camX;
    private float camY;
    public float initialCamX = 0;
    public float initialCamY = 0;
    public float camShakeMultiplier = 0.1f;
    private GameObject player;

    private bool pause = false;
    public Volume globalVolume;
    private DepthOfField blurEffect;

    public GameObject pauseCanvas;

    public float distanceIncrementRatio = 0.05f;
    private float rawDistance;
    public static int distance;

    double realtime;
    double deathWaitTime;
    double distanceUpdateTime;

    float distanceUpdateValue = 1;

    bool playerIsDead;

    private GameObject speedReference;

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

    public void ExitHandler()
    {
        Application.Quit();
    }

    public void RestartHandler()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MenuHandler()
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

    private void GameStop(float timeScale, bool pauseState, bool blurEffectState)
    {
        // animation reset so it starts from scratch when game paused
        pauseCanvas.transform.GetChild(3).gameObject.GetComponent<Animator>().Play("distancePanel", 0, 0.0f);
        pauseCanvas.transform.GetChild(2).gameObject.GetComponent<Animator>().Play("pauseButton", 0, 0.0f);

        Time.timeScale = timeScale;
        pause = pauseState;
        blurEffect.active = blurEffectState;
    }

    private void PauseGame()
    {
        // animation reset so it starts from scratch when game paused
        pauseCanvas.transform.GetChild(3).gameObject.GetComponent<Animator>().Play("distancePanel", 0, 0.0f);
        pauseCanvas.transform.GetChild(2).gameObject.GetComponent<Animator>().Play("pauseButton", 0, 0.0f);

        GameStop(0f, true, true);
        pauseCanvas.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void ResumeGame()
    {
        GameStop(1f, false, false);
        pauseCanvas.transform.GetChild(0).gameObject.SetActive(false);
        pauseCanvas.transform.GetChild(1).gameObject.SetActive(false);
    }

    void PlayerDead(Notification notificacion)
    {
        if (!playerIsDead)
        {
            deathWaitTime = realtime;
            playerIsDead = true;
        }
        else if (realtime - deathWaitTime >= 3)
        {
            GameStop(0f, true, true);
            pauseCanvas.transform.GetChild(1).gameObject.SetActive(true);
            pauseCanvas.transform.GetChild(2).gameObject.GetComponentInChildren<Button>().interactable = false;
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        speedReference = GameObject.FindGameObjectWithTag("Background");

        pauseCanvas.transform.GetChild(0).gameObject.SetActive(false);
        pauseCanvas.transform.GetChild(1).gameObject.SetActive(false);

        if (globalVolume.profile.TryGet(out DepthOfField tmp))
        {
            blurEffect = tmp;
        }

        blurEffect.gaussianMaxRadius = new ClampedFloatParameter(1,0,1);

        // reset camera values so time.scale resets its value when level reloads
        GameStop(1f, false, false);

        NotificationCenter.DefaultCenter().AddObserver(this, "PlayerDead");
    }


    void Update()
    {

        // --- CONFIGURATIONS --- //

        realtime = Time.timeSinceLevelLoad;



        // Camera shake after player gets hit

        if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hurt") && !pause)
        {
            CameraShake(true);
        }
        else
        {
            CameraShake(false);
        }

        // Game pause notifications

        if (pause)
        {
            NotificationCenter.DefaultCenter().PostNotification(this, "GamePaused");
        }
        else
        {
            NotificationCenter.DefaultCenter().PostNotification(this, "GameNotPaused");

        }

        // Distance measurements

        if (!pause && !player.GetComponent<Animator>().GetBool("death"))
        {
            // old method

            //if (!distanceAddReset)  
            //{                       
            //    rawDistance += distanceIncrementRatio * player.GetComponent<Animator>().speed;
            //    distanceAddPrevNum = Mathf.RoundToInt(Convert.ToSingle(realtime*10));
            //    distanceAddReset = true;
            //}

            //if (distanceAddReset && Mathf.RoundToInt(Convert.ToSingle(realtime)) != distanceAddPrevNum)
            //{
            //    distanceAddReset = false;
            //}



            // this setup makes sure this adds values to the distance value based on real time at a maximum of 0.2 seconds refresh rate
            // (this is to avoid distance not being incremented depending on the device refresh ratio but on a fixed rate)

            distanceUpdateValue = 1 - ((speedReference.GetComponent<Animator>().speed - 0.7f) * 0.8f);


            if (realtime - distanceUpdateTime >= distanceUpdateValue)
            {
                rawDistance += 1;
                distanceUpdateTime = realtime;
            }
        }
        

        distance = Mathf.RoundToInt(rawDistance);

        pauseCanvas.transform.GetChild(3).transform.GetChild(0).GetComponent<TMP_Text>().text = distance + " m"; // distance printed in UI (up-left corner)
        pauseCanvas.transform.GetChild(1).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = distance + " m"; // distance printed in UI (Dead UI)

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
