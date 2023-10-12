using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{

    private Animator animator;
    private Rigidbody2D body;
    private GameObject player;
    private BoxCollider2D boxCollider;
    private float randomValue;
    public float jumpValue = 22;
    public float prepareSpeedValue = 5;

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
        if (randomValue <= 0.5f)
        {
            if (Mathf.Abs(transform.position.x - player.transform.position.x) <= 18)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
                {
                    body.velocity = new Vector2(-17, body.velocity.y);
                }
                else
                {
                    body.velocity = new Vector2(body.velocity.x + prepareSpeedValue, body.velocity.y);
                }
                

                if (!animator.GetBool("prepare"))
                {
                    GetComponent<AudioSource>().Stop();
                    transform.GetChild(0).GetComponent<AudioSource>().Play();
                    animator.SetBool("prepare", true);
                }              

                if (Mathf.Abs(transform.position.x - player.transform.position.x) <= 5 && boxCollider.IsTouchingLayers(groundLayer))
                {
                    if (!animator.GetBool("jump"))
                    {
                        transform.GetChild(1).GetComponent<AudioSource>().Play();
                        animator.SetBool("jump", true);
                    }
                    
                    body.velocity = new Vector2(body.velocity.x, jumpValue);
                }
            }

            
        }
        
    }
}
