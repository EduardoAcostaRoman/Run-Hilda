using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : MonoBehaviour
{
    private GameObject speedReference;
    private Rigidbody2D body;
    private Animator animator;

    public float speed = 11;
    public Vector3 startingPosition;

    public GameObject explosion;
    public Vector3 explosionPosOffset;

    float lastVelocityValueX;

    bool playerIsDead;

    

    private void OnTriggerEnter2D(Collider2D collisionObject)
    {

        // For projectile collision
        if (collisionObject.tag == "PlayerProjectile")
        {
            Instantiate(explosion, new Vector3(transform.position.x + explosionPosOffset.x, 
                                               transform.position.y + explosionPosOffset.y,
                                               transform.position.z + explosionPosOffset.z), 
                                               explosion.transform.rotation);
            Object.Destroy(gameObject);
        }
    }

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
