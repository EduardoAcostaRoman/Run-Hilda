using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;
    private GameObject player;
    private BoxCollider2D boxCollider;

    double realTime;
    double standResetTime;

    bool standReset;

    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        realTime = Time.timeSinceLevelLoad;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("stand") && !standReset)
        {
            standReset = true;
        }
        else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("stand") && standReset)
        {
            standReset = false;
        }
    }
}
