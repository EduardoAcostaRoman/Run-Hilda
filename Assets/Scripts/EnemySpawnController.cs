using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro.Examples;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{

    public GameObject rhyhorn;
    public GameObject wolf;
    public GameObject bat;
    public GameObject pterodactyl;

    public GameObject demon;

    bool bossSpawned;

    float realtime;
    float prevtime;

    private float randomValue;

    public float spawnRate = 3;

    private GameObject player;

    private bool spawnTrigger = true;
    public int bossDistanceToSpawn = 100;
    int bossPreviousDistanceCount;
    bool bossToBeSpawned;

    void BossDefeated(Notification notificacion)
    {
        // when boss is defeated, the meter count resets and starts counting fresh again to spawn a new boss instance
        bossPreviousDistanceCount = CameraController.distance;
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        NotificationCenter.DefaultCenter().AddObserver(this, "BossDefeated");
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
            // boss enemy check (so there is only one at a time)
            bossSpawned = false;

            GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in Enemies)
            {
                if (enemy.name.Contains("Demon"))
                {
                    bossSpawned = true;
                }
            }

            if (!bossSpawned)
            {
                // if no boss instantiated, spawns enemies randomly
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

                //this way, the boss spawns every X meters
                if (CameraController.distance - bossPreviousDistanceCount >= bossDistanceToSpawn)
                {
                    Instantiate(demon, demon.transform.position, demon.transform.rotation);
                    NotificationCenter.DefaultCenter().PostNotification(this, "BossSpawned");
                }    
            }

            

            prevtime = realtime;
        }
    }
}
