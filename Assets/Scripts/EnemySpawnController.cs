using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{

    public GameObject rhyhorn;
    public GameObject wolf;

    float realtime;
    float prevtime;

    public float spawnRate = 3;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        // --- CONFIGURATIONS --- //

        realtime = Time.fixedTime;

        // For PC tests (delete/comment for build)

        if (Input.GetKeyDown(KeyCode.S))
        {
            Instantiate(rhyhorn, new Vector3(14, -3.35f, -1), rhyhorn.transform.rotation);
        }

        // For PC tests (delete/comment for build)

        // --- SPAWN ALGORITHM --- //

        if (realtime - prevtime >= spawnRate && !player.GetComponent<Animator>().GetBool("death"))
        {
            // Instantiate(rhyhorn, new Vector3(14, -3.35f, -1), rhyhorn.transform.rotation);
            Instantiate(wolf, new Vector3(14, -3.35f, -1), wolf.transform.rotation);
            prevtime = realtime;
        }

    }
}
