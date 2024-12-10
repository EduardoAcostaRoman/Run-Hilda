using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject buffItem;
    public GameObject puffItem;
    public double buffItemSpawnTime = 3;

    private double realtime;
    private double prevSpawnTime;

    private GameObject player;

    private bool buffActivated = false;
    private bool puffActivated = false;

    private float randomValue;


    void BuffActivated(Notification notificacion)
    {
        buffActivated = true;
    }

    void BuffNotActivated(Notification notificacion)
    {
        buffActivated = false;
    }

    void PuffActivated(Notification notificacion)
    {
        puffActivated = true;
    }

    void PuffNotActivated(Notification notificacion)
    {
        puffActivated = false;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        NotificationCenter.DefaultCenter().AddObserver(this, "BuffActivated");
        NotificationCenter.DefaultCenter().AddObserver(this, "BuffNotActivated");

        NotificationCenter.DefaultCenter().AddObserver(this, "PuffActivated");
        NotificationCenter.DefaultCenter().AddObserver(this, "PuffNotActivated");
    }

    void Update()
    {
        realtime = Time.timeSinceLevelLoad;
        randomValue = Random.value;


        // --- SPAWN CONTROL --- //

        // buff item control
        
        if (realtime - prevSpawnTime > buffItemSpawnTime && !player.GetComponent<Animator>().GetBool("death"))
        {
            if (randomValue <= 0.5f && !buffActivated)
            {
                Instantiate(buffItem, new Vector3(13, 1.4f, buffItem.transform.position.z), buffItem.transform.rotation);
            }
            else if (!puffActivated)
            {
                Instantiate(puffItem, new Vector3(13, 1.4f, puffItem.transform.position.z), puffItem.transform.rotation);
            }
            
            prevSpawnTime = realtime;
        }

        //to reset timer when all activated
        if (buffActivated && puffActivated)
        {
            prevSpawnTime = realtime;
        }
    }
}
