using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpeedController : MonoBehaviour
{
    public GameObject player;
    private Animator animator;

    public float speed = 0.7f;
    public float speedTimeIncrementRatio = 0.001f;
    public float speedValueIncrementRatio = 0.001f;
    public float speedMax = 1.7f;
    public float speedMin = 0.7f;

    private bool gameIsPaused;

    private bool playerIsDead;

    private bool speedAddReset;
    private int speedAddPrevNum;

    double realtime;
    double speedUpdateTime;



    void GamePaused(Notification notificacion)
    {
        gameIsPaused = true;
    }

    void GameNotPaused(Notification notificacion)
    {
        gameIsPaused = false;
    }

    //void PlayerDead(Notification notificacion)
    //{
    //    playerIsDead = true; 
    //}

    void Start()
    {
        animator = GetComponent<Animator>();

        NotificationCenter.DefaultCenter().AddObserver(this, "GamePaused");
        NotificationCenter.DefaultCenter().AddObserver(this, "GameNotPaused");

        // NotificationCenter.DefaultCenter().AddObserver(this, "PlayerDead");
    }

   
    void Update()
    {

        // --- CONFIGURATIONS --- //

        realtime = Time.timeSinceLevelLoad;

        // manual speed control testing
        if (Input.GetKey(KeyCode.A))
        {
            if (speed <= speedMin)
            {
                speed = speedMin;
            }
            else
            {
                speed -= 0.01f;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (speed >= speedMax)
            {
                speed = speedMax;
            }
            else
            {
                speed += 0.01f;
            }
        }

        // background speed
        animator.speed = speed;


        //// player speed
        //if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hurt"))
        //{
        //    player.GetComponent<Animator>().speed = 1;
        //    speed = speedMin;
        //}
        //else if (player.GetComponent<Animator>().GetBool("death"))
        if (player.GetComponent<Animator>().GetBool("death"))
        {
            speed = Mathf.Lerp(speed, 0, 0.1f);
        }
        else if (!gameIsPaused)    // ensures game speed doesn't increase while game is paused
        {
            // speed clamp (min & max)
            player.GetComponent<Animator>().speed = speed;
            if (speed >= speedMax)
            {
                speed = speedMax;
            }
            else
            {
                // old method

                //if (!speedAddReset)  
                //{                       
                //    speed += speedIncrementRatio;
                //    speedAddPrevNum = Mathf.RoundToInt(Convert.ToSingle(realtime * 10));
                //    speedAddReset = true;
                //}

                //if (speedAddReset && Mathf.RoundToInt(Convert.ToSingle(realtime)) != speedAddPrevNum)
                //{
                //    speedAddReset = false;
                //}

                // this setup makes sure this adds values to the distance value using real time 
                // (this is to avoid distance not being incremented depending on the device refresh ratio but on a fixed rate)

                if (realtime - speedUpdateTime >= speedTimeIncrementRatio)
                {
                    speed += speedValueIncrementRatio;
                    speedUpdateTime = realtime;
                }
            }

            
        }

        // enemies speed
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Animator>().speed = speed;
        }

        // buff item speed
        foreach (GameObject buffItem in GameObject.FindGameObjectsWithTag("BuffItem"))
        {
            buffItem.GetComponent<Rigidbody2D>().linearVelocity = new Vector2((speed + 0.3f) * -11, 0);
        }

        // puff item speed
        foreach (GameObject puffItem in GameObject.FindGameObjectsWithTag("PuffItem"))
        {
            puffItem.GetComponent<Rigidbody2D>().linearVelocity = new Vector2((speed + 0.3f) * -11, 0);
        }

    }
}
