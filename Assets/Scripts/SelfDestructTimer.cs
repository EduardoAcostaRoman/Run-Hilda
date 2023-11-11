using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructTimer : MonoBehaviour
{
    public float selfDestructTime = 1;

    void Start()
    {
        Destroy(gameObject, selfDestructTime);
    }

    void Update()
    {
        
    }
}
