using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death2Effect : MonoBehaviour
{
    private float effectVelocity;
    public float velocityIncrement = 0.001f;
    private float distanceAngle;

    private GameObject player;

    void Start()
    {
        distanceAngle = transform.GetChild(0).transform.position.x;
        player = GameObject.FindGameObjectWithTag("Player");
    }

   
    void Update()
    {
        effectVelocity += velocityIncrement;

        transform.position = new Vector3(player.transform.position.x + (effectVelocity * Mathf.Sin(distanceAngle)),
                    player.transform.position.y + 1.08f + (effectVelocity * Mathf.Cos(distanceAngle)),
                    player.transform.position.z);

        Destroy(gameObject, 5);
    }
}
