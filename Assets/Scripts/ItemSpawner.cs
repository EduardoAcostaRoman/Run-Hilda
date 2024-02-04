using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject buffItem;
    public double buffItemSpawnTime = 5;

    private double realtime;
    private double prevSpawnTime;

    private bool spawnTrigger = true;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        realtime = Time.fixedTime;

        if (!CharacterMainController.buffTrigger)
        {
            if (realtime - prevSpawnTime > buffItemSpawnTime)
            {
                Instantiate(buffItem, new Vector3(13, 1.4f, buffItem.transform.position.z), buffItem.transform.rotation);
                prevSpawnTime = realtime;
            }
        }



        // --- SPAWN CONTROL --- //

        // buff item control
        if (!CharacterMainController.buffTrigger && !player.GetComponent<Animator>().GetBool("death"))
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
