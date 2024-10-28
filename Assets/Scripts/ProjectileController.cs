using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float velocityX = 0;
    public float velocityY = 0;

    public string layerToDestroyInContactWith = "Enemy";

    private Rigidbody2D body;

    public GameObject destroyEffect;

    private void destroyObject()
    {
        Instantiate(destroyEffect,
                    new Vector3(transform.position.x, transform.position.y, destroyEffect.transform.position.z),
                    transform.rotation);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collisionObject)
    {
        // For enemy collision
        if (collisionObject.tag == "Enemy" && layerToDestroyInContactWith == "Enemy")
        {
            destroyObject();
        }

        // For player collision
        if (collisionObject.tag == "Player" && layerToDestroyInContactWith == "Player")
        {
            destroyObject();
        }
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        Instantiate(destroyEffect,
                    new Vector3(transform.position.x, transform.position.y, destroyEffect.transform.position.z),
                    transform.rotation);

        body.linearVelocity = new Vector2(velocityX, velocityY);
    }

    void Update()
    {
        
    }
}
