using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puff : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D body;
    private Animator animator;

    public GameObject puffBall;
    public float puffBallOffsetX = 0;
    public float puffBallOffsetY = 0;
    public float puffBallSpawnRate = 3;

    private double realTime;
    private double timeReset;

    private bool gamePaused = false;

    private bool puffActivated = false;

    void PuffActivated(Notification notificacion)
    {
        puffActivated = true;
    }

    void PuffNotActivated(Notification notificacion)
    {
        puffActivated = false;
    }

    void GamePaused(Notification notificacion)
    {
        gamePaused = true;
    }

    void GameNotPaused(Notification notificacion)
    {
        gamePaused = false;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        realTime = Time.timeSinceLevelLoad;

        if (!puffActivated)
        {
            timeReset = realTime;
        }

        if (realTime - timeReset >= puffBallSpawnRate)
        {
            animator.SetBool("attack", true);
            Instantiate(puffBall, new Vector3(transform.position.x + puffBallOffsetX,
                                              transform.position.y + puffBallOffsetY,
                                              puffBall.transform.position.z),
                                              puffBall.transform.rotation);
            timeReset = realTime;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (animator.GetBool("attack"))
            {
                transform.GetChild(4).GetComponent<AudioSource>().Play();
            }
            animator.SetBool("attack", false);
        }
    }
}
