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
    public float speedValueX = -17f;
    public float attackSpeedValueY = -6;

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

        body.gravityScale = 0;

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
            body.velocity = new Vector2(body.velocity.x, attackSpeedValueY);
            body.gravityScale = -5;
        }

        if (body.velocityY > 0)
        {
            animator.SetBool("flyUp", true);
        }

        body.velocity = new Vector2(speedValueX, body.velocity.y);
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
