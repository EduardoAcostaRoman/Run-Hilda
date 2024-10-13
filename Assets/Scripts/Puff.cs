using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puff : MonoBehaviour
{
    private float startPosX;
    private float startPosY;
    public float posOffsetX = 1.43f;
    public float posOffsetY = 3.11f;
    private GameObject player;
    private Rigidbody2D body;

    private bool gamePaused = false;

    private bool buffActivated = false;

    public float changeRateAxisX = 0.1f;
    public float changeRateAxisY = 0.1f;

    public float changeRateRetreat = 0.1f;

    void BuffActivated(Notification notificacion)
    {
        buffActivated = true;
    }

    void BuffNotActivated(Notification notificacion)
    {
        buffActivated = false;
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

        NotificationCenter.DefaultCenter().AddObserver(this, "GamePaused");
        NotificationCenter.DefaultCenter().AddObserver(this, "GameNotPaused");

        NotificationCenter.DefaultCenter().AddObserver(this, "BuffActivated");
        NotificationCenter.DefaultCenter().AddObserver(this, "BuffNotActivated");
    }

    // Update is called once per frame
    void Update()
    {
        startPosX = player.transform.position.x - posOffsetX;
        startPosY = player.transform.position.y + posOffsetY;

        if (!gamePaused)
        {
            if (buffActivated)
            {
                //transform.position = new Vector3(Mathf.Lerp(transform.position.x, startPosX, 0.04f),
                //                             Mathf.Lerp(transform.position.y, startPosY, 0.1f),
                //                             transform.position.z);

                transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, startPosX, changeRateAxisX),
                                             Mathf.MoveTowards(transform.position.y, startPosY, changeRateAxisY),
                                             transform.position.z);
            }
            else
            {
                //transform.position = new Vector3(Mathf.Lerp(transform.position.x, -13, 0.02f),
                //                             transform.position.y,
                //                             transform.position.z);

                transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, -13, changeRateRetreat),
                                             transform.position.y,
                                             transform.position.z);
            }
        }


        if (player.GetComponent<CharacterMainController>().blink || player.GetComponent<Animator>().GetBool("death"))
        {
            GetComponent<Animator>().SetBool("hurt", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("hurt", false);
        }
    }
}
