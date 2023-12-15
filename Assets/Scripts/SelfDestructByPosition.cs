using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructByPosition : MonoBehaviour
{
    public float positionX;
    public float positionY;

    public bool onlyAxisX;

    public bool greaterThanPosition;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (onlyAxisX)
        {
            if (!greaterThanPosition)
            {
                if (transform.position.x <= positionX)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (transform.position.x >= positionX)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (!greaterThanPosition)
            {
                if (transform.position.y <= positionY)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (transform.position.y >= positionY)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
