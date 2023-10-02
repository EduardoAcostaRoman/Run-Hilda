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
    void Start()
    {
        animator = GetComponent<Animator>();
    }

   
    void Update()
    {
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

        animator.speed = speed;
        if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hurt"))
        {
            player.GetComponent<Animator>().speed = 1;
            speed = speedMin;
        }
        else
        {
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

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Animator>().speed = speed;
        }
        
    }
}
