using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float velocityX = 0;
    public float velocityY = 0;

    public string layerToDestroyInContactWith = "Enemy";

    private Rigidbody2D body;

    private void OnTriggerEnter2D(Collider2D collisionObject)
    {

        // For enemy collision
        if (collisionObject.tag == "Enemy" && layerToDestroyInContactWith == "Enemy")
        {
            Destroy(gameObject);
        }

        // For player collision
        if (collisionObject.tag == "Player" && layerToDestroyInContactWith == "Player")
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.linearVelocity = new Vector2(velocityX, velocityY);
    }

    void Update()
    {
        
    }
}
