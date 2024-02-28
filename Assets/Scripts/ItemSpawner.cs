using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject buffItem;
    public double buffItemSpawnTime = 3;

    private double realtime;
    private double prevSpawnTime;

    private GameObject player;

    private bool buffActivated = false;

    void BuffActivated(Notification notificacion)
    {
        buffActivated = true;
    }

    void BuffNotActivated(Notification notificacion)
    {
        buffActivated = false;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        NotificationCenter.DefaultCenter().AddObserver(this, "BuffActivated");
        NotificationCenter.DefaultCenter().AddObserver(this, "BuffNotActivated");
    }

    void Update()
    {
        realtime = Time.timeSinceLevelLoad;

        if (buffActivated)
        {
            if (realtime - prevSpawnTime > buffItemSpawnTime)
            {
                Instantiate(buffItem, new Vector3(13, 1.4f, buffItem.transform.position.z), buffItem.transform.rotation);
                prevSpawnTime = realtime;
            }
        }



        // --- SPAWN CONTROL --- //

        // buff item control
        if (!buffActivated && !player.GetComponent<Animator>().GetBool("death"))
        {
            if (realtime - prevSpawnTime > buffItemSpawnTime)
            {
                Instantiate(buffItem, new Vector3(13, 1.4f, buffItem.transform.position.z), buffItem.transform.rotation);
                prevSpawnTime = realtime;
            }
        }
        else
        {
            prevSpawnTime = realtime;
        }



    }
}
