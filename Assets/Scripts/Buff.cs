using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    private float startPosX;
    private float startPosY;
    public float posOffsetX = 1.43f;
    public float posOffsetY = 3.11f;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    
    void Update()
    {
        startPosX = player.transform.position.x - posOffsetX;
        startPosY = player.transform.position.y + posOffsetY;

        if (!CameraController.pause)
        {
            if (CharacterMainController.buffTrigger)
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, startPosX, 0.04f),
                                             Mathf.Lerp(transform.position.y, startPosY, 0.1f),
                                             transform.position.z);
            }
            else
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, -13, 0.02f),
                                             Mathf.Lerp(transform.position.y, 0.62f, 0.1f),
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
