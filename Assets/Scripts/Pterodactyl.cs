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
    public float attackSpeedValue = -6;

    private bool wingSoundReset;

    public LayerMask groundLayer;

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
        if (Mathf.Abs(transform.position.x - player.transform.position.x) <= attackDistanceValue && !boxCollider.IsTouchingLayers(groundLayer))
        {

            //transform.GetChild(1).GetComponent<AudioSource>().Play();
            animator.SetBool("attack", true);

            body.velocity = new Vector2(body.velocity.x, attackSpeedValue);
        }

        if (boxCollider.IsTouchingLayers(groundLayer))
        {
            animator.SetBool("flyUp", true);
            body.velocity = new Vector2(body.velocity.x, -attackSpeedValue);
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
