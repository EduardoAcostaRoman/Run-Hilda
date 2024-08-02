using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : MonoBehaviour
{
    public float speed = 11;
    private GameObject speedReference;
    private Rigidbody2D body;

    float lastVelocityValueX;

    bool playerIsDead;

    public Vector3 startingPosition;

    void PlayerDead(Notification notificacion)
    {
        playerIsDead = true;
    }

    void Start()
    {
        speedReference = GameObject.FindGameObjectWithTag("Background");
        body = GetComponent<Rigidbody2D>();

        NotificationCenter.DefaultCenter().AddObserver(this, "PlayerDead");

        gameObject.tag = "Enemy";

        transform.position = startingPosition;
    }

    
    void Update()
    {
        if (!playerIsDead)
        {
            body.velocity = new Vector2((speedReference.GetComponent<Animator>().speed + 0.3f) * -speed, body.velocity.y);
            lastVelocityValueX = body.velocityX;
        }
        else
        {
            body.velocity = new Vector2(lastVelocityValueX, body.velocity.y);
        }
        


        if (transform.position.x <= -18)
        {
            Object.Destroy(gameObject);
        }
    }
}
