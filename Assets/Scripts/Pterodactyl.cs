using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pterodactyl : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;
    private GameObject player;
    private BoxCollider2D boxCollider;
    private float randomValue;
    public float attackDistanceValue = 5;
    public float attackSpeedValueY = -6;
    public float attackVelocityDecreaseRatio = 0.1f;
    public float attackSpeedValueX = -17;

    private bool wingSoundReset;

    public LayerMask groundLayer;

    private double realTime;
    private double timeReset;

    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        randomValue = Random.value;

        GetComponent<AudioSource>().Play();
    }


    void Update()
    {
        realTime = Time.timeSinceLevelLoad;

        if (Mathf.Abs(transform.position.x - player.transform.position.x) <= attackDistanceValue && !animator.GetBool("attack"))
        {

            //transform.GetChild(1).GetComponent<AudioSource>().Play();
            timeReset = realTime;
            animator.SetBool("attack", true);
        }

        if (body.velocityY > 0)
        {
            animator.SetBool("flyUp", true);
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
        {
            if (realTime - timeReset >= 0.01f)
            {
                attackSpeedValueY += attackVelocityDecreaseRatio;
                timeReset = realTime;
            }

            body.velocity = new Vector2(0, 0);
            body.velocity = new Vector2(attackSpeedValueX, attackSpeedValueY);
        }
    }

    private void FixedUpdate()
    {
        if (GetComponent<SpriteRenderer>().sprite.name == "fly_0" && !wingSoundReset)
        {
            GetComponent<AudioSource>().Play();
            wingSoundReset = true;
        }
        else if (GetComponent<SpriteRenderer>().sprite.name == "fly_5")
        {
            wingSoundReset = false;
        }
    }
}
