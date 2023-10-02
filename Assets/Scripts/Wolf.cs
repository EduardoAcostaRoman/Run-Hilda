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

    public LayerMask groundLayer;

    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        randomValue = Random.value;
    }

    
    void Update()
    {
        if (randomValue <= 0.5f)
        {
            if (Mathf.Abs(transform.position.x - player.transform.position.x) <= 13)
            {
                animator.SetBool("prepare", true);

                if (Mathf.Abs(transform.position.x - player.transform.position.x) <= 5 && boxCollider.IsTouchingLayers(groundLayer))
                {
                    animator.SetBool("jump", true);
                    body.velocity = new Vector2(body.velocity.x, jumpValue);
                }
            }
        }
        
    }
}
