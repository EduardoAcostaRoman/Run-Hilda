using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death2Effect : MonoBehaviour
{
    private float effectVelocity;
    public float velocityIncrement = 0.001f;
    private float distanceAngle;

    private Rigidbody2D body;

    private GameObject player;

    void Start()
    {
        distanceAngle = transform.GetChild(0).transform.position.x;
        player = GameObject.FindGameObjectWithTag("Player");
        body = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {
        effectVelocity += velocityIncrement;

        //transform.position = new Vector3(player.transform.position.x + (effectVelocity * Mathf.Sin(distanceAngle)),
        //            player.transform.position.y + 1.08f + (effectVelocity * Mathf.Cos(distanceAngle)),
        //            player.transform.position.z);

        switch (distanceAngle)
        {
            case 1:
                body.velocity = new Vector2(0, 5);
                break;
            case 2:
                body.velocity = new Vector2(3.5f, 3.5f);
                break;
            case 3:
                body.velocity = new Vector2(5, 0);
                break;
            case 4:
                body.velocity = new Vector2(3.5f, -3.5f);
                break;
            case 5:
                body.velocity = new Vector2(0, -5);
                break;
            case 6:
                body.velocity = new Vector2(-3.5f, -3.5f);
                break;
            case 7:
                body.velocity = new Vector2(-5, 0);
                break;
            case 8:
                body.velocity = new Vector2(-3.5f, 3.5f);
                break;
            default:
                body.velocity = new Vector2(3.5f, 3.5f);
                break;
        }

        Destroy(gameObject, 5);
    }
}
