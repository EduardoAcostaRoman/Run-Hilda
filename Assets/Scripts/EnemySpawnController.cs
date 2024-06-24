using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{

    public GameObject rhyhorn;
    public GameObject wolf;
    public GameObject bat;

    float realtime;
    float prevtime;

    private float randomValue;

    public float spawnRate = 3;

    private GameObject player;



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        // --- CONFIGURATIONS --- //

        realtime = Time.timeSinceLevelLoad;
        randomValue = Random.value;

        // For PC tests (delete/comment for build)

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Instantiate(rhyhorn, new Vector3(14, -3.35f, -1), rhyhorn.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(wolf, new Vector3(14, -3.35f, -1), wolf.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Instantiate(bat, new Vector3(14, -1f, -1), bat.transform.rotation);
        }

        // For PC tests (delete/comment for build)

        // --- SPAWN ALGORITHM --- //

        if (realtime - prevtime >= spawnRate && !player.GetComponent<Animator>().GetBool("death"))
        {
            // Instantiate(rhyhorn, new Vector3(14, -3.35f, -1), rhyhorn.transform.rotation);
            // Instantiate(wolf, new Vector3(14, -3.35f, -1), wolf.transform.rotation);
            

            if (randomValue >= 0.5f)
            {
                Instantiate(bat, new Vector3(14, -1f, -1), bat.transform.rotation);
            }
            else
            {
                Instantiate(wolf, new Vector3(14, -3.35f, -1), wolf.transform.rotation);
            }

            prevtime = realtime;
        }

    }
}
