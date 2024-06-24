using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    private float randomValue;

    private bool wingSoundReset;

    void Start()
    {
        randomValue = Random.value;

        if (randomValue >= 0.5f)
        {
            transform.position = new Vector3(transform.position.x, -1f, transform.position.z);
        }
        else 
        {
            transform.position = new Vector3(transform.position.x, 1.3f, transform.position.z);
        }
    }

    
    void Update()
    {
        
        


    }

    private void FixedUpdate()
    {
        if (GetComponent<SpriteRenderer>().sprite.name == "bat_4" && !wingSoundReset)
        {
            GetComponent<AudioSource>().Play();
            wingSoundReset = true;
        }
        else if (GetComponent<SpriteRenderer>().sprite.name == "bat_5")
        {
            wingSoundReset = false;
        }
    }
}
