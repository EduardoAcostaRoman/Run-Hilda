using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : MonoBehaviour
{
    public float speed = 11;
    private GameObject speedReference;
    private Rigidbody2D body;

    void Start()
    {
        speedReference = GameObject.FindGameObjectWithTag("Background");
        body = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        body.velocity = new Vector2((speedReference.GetComponent<Animator>().speed + 0.3f) * -speed, body.velocity.y);

        if (transform.position.x <= -18)
        {
            Object.Destroy(gameObject);
        }
    }
}
