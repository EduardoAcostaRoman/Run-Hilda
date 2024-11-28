using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    private float activatedPosX;
    private float activatedPosY;
    public float posOffsetX = 1.43f;
    public float posOffsetY = 3.11f;
    public float startPosX = -11;
    public float startPosY = 6.2f;

    private GameObject player;
    private Rigidbody2D body;

    private bool gamePaused = false;

    private bool buffActivated = false;
    private bool buffPlayerPosReached = false;

    public float changeRateAxisX = 5;
    public float changeRateAxisY = 5;

    public float changeRateRetreatX = 2;
    public float changeRateRetreatY = 2;

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

    private void FixedUpdate()
    {
        activatedPosX = player.transform.position.x - posOffsetX;
        activatedPosY = player.transform.position.y + posOffsetY;


        // Clamps buff position
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, startPosX, activatedPosX),
                                         Mathf.Clamp(transform.position.y, activatedPosY, startPosY),
                                         transform.position.z);

        if (!gamePaused)
        {
            if (buffActivated)
            {
                // to make sure the buff moves along with the player once it reaches the final position point
                if (transform.position.x >= (activatedPosX - 0.1f) && transform.position.y <= (activatedPosY + 0.1f) || buffPlayerPosReached)
                {
                    transform.position = new Vector3(activatedPosX, activatedPosY, transform.position.z);
                    body.linearVelocity = player.GetComponent<Rigidbody2D>().linearVelocity;
                    buffPlayerPosReached = true;
                }
                else
                {
                    body.linearVelocity = new Vector2(changeRateAxisX, -changeRateAxisY);
                }
            }
            else
            {
                body.linearVelocity = new Vector2(-changeRateRetreatX, changeRateRetreatY);
            }
        }
    }


    void Update()
    {
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
