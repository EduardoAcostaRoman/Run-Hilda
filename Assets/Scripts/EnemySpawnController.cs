using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{

    public GameObject rhyhorn;
    public GameObject wolf;
    public GameObject bat;
    public GameObject pterodactyl;

    float realtime;
    float prevtime;

    private float randomValue;

    public float spawnRate = 3;

    private GameObject player;

    private bool spawnTrigger = true;



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

        if (Input.GetKeyDown(KeyCode.S)) // stops/starts spawning enemies
        {
            spawnTrigger = !spawnTrigger;
        }

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

        if (realtime - prevtime >= spawnRate && !player.GetComponent<Animator>().GetBool("death") && spawnTrigger)
        {
            // POSITION SAMPLES:
            // Instantiate(rhyhorn, new Vector3(14, -3.35f, -1), rhyhorn.transform.rotation);
            // Instantiate(wolf, new Vector3(14, -3.35f, -1), wolf.transform.rotation);
            


            if (randomValue <= 0.33f)
            {
                Instantiate(bat, bat.transform.position, bat.transform.rotation);
            }
            else if (randomValue <= 0.66f)
            {
                Instantiate(wolf, wolf.transform.position, wolf.transform.rotation);
            }
            else
            {
                Instantiate(pterodactyl, pterodactyl.transform.position, pterodactyl.transform.rotation);
            }

            prevtime = realtime;
        }

    }
}
