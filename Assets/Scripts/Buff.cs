using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    private float activatedPosX;
    private float activatedPosY;
    private float randomValue;
    public float posOffsetX = 1.43f;
    public float posOffsetY = 3.11f;
    public float startPosX = -11;
    public float startPosY = 6.2f;

    private GameObject player;
    private Rigidbody2D body;

    private bool gamePaused = false;

    private bool buffActivated = false;
    private bool puffActivated = false;
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

        NotificationCenter.DefaultCenter().AddObserver(this, "GamePaused");
        NotificationCenter.DefaultCenter().AddObserver(this, "GameNotPaused");

        NotificationCenter.DefaultCenter().AddObserver(this, "BuffActivated");
        NotificationCenter.DefaultCenter().AddObserver(this, "BuffNotActivated");

        NotificationCenter.DefaultCenter().AddObserver(this, "PuffActivated");
        NotificationCenter.DefaultCenter().AddObserver(this, "PuffNotActivated");
    }

    private void FixedUpdate()
    {
        activatedPosX = player.transform.position.x - posOffsetX;
        activatedPosY = player.transform.position.y + posOffsetY;

        randomValue = Random.value;


        // Clamps buff position
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, startPosX, activatedPosX),
                                         Mathf.Clamp(transform.position.y, activatedPosY, startPosY),
                                         transform.position.z);

        if (!gamePaused)
        {
            if ((gameObject.tag == "Buff" && buffActivated) || (gameObject.tag == "Puff" && puffActivated))
            {
                // to make sure the buff moves along with the player once it reaches the final position point
                if ((transform.position.x >= (activatedPosX - 0.1f) && transform.position.y <= (activatedPosY + 0.1f)) || buffPlayerPosReached)
                {
                    transform.position = new Vector3(activatedPosX, activatedPosY, transform.position.z);
                    body.linearVelocity = player.GetComponent<Rigidbody2D>().linearVelocity;

                    if (!buffPlayerPosReached)
                    {
                        if (randomValue >= 0.5f)
                        {
                            transform.GetChild(0).GetComponent<AudioSource>().Play();
                        }
                        else
                        {
                            transform.GetChild(1).GetComponent<AudioSource>().Play();
                        }
                        
                        buffPlayerPosReached = true;
                    }
                    
                }
                else
                {
                    body.linearVelocity = new Vector2(changeRateAxisX, -changeRateAxisY);
                }
            }
            else
            {
                body.linearVelocity = new Vector2(-changeRateRetreatX, changeRateRetreatY);
                buffPlayerPosReached = false;
            }
        }
    }


    void Update()
    {
        if (player.GetComponent<CharacterMainController>().blink || player.GetComponent<Animator>().GetBool("death"))
        {
            if (!GetComponent<Animator>().GetBool("hurt") && (gameObject.tag == "Buff" && buffActivated) || (gameObject.tag == "Puff" && puffActivated))
            {
                if (randomValue >= 0.5f)
                {
                    transform.GetChild(2).GetComponent<AudioSource>().Play();
                }
                else
                {
                    transform.GetChild(3).GetComponent<AudioSource>().Play();
                }
            }

            GetComponent<Animator>().SetBool("hurt", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("hurt", false);
        }
    }
}
