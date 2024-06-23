using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    public GameObject player;
    private Animator animator;

    public float speed = 0.7f;
    public float speedIncrementRatio = 0.001f;
    public float speedMax = 1.7f;
    public float speedMin = 0.7f;

    private bool gameIsPaused;

    private bool playerIsDead;

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


        // player speed
        if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hurt"))
        {
            player.GetComponent<Animator>().speed = 1;
            speed = speedMin;
        }
        else if (player.GetComponent<Animator>().GetBool("death"))
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
                speed += speedIncrementRatio;
            }
        }

        // enemies speed
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Animator>().speed = speed;
        }

        // buff item speed
        foreach (GameObject buffItem in GameObject.FindGameObjectsWithTag("Buff"))
        {
            buffItem.GetComponent<Rigidbody2D>().velocity = new Vector2((speed + 0.3f) * -11, 0);
        }

    }
}
