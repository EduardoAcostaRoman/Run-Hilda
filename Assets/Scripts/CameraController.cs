using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float angle;
    private float camX;
    private float camY;
    public float initialCamX = 0;
    public float initialCamY = 0;
    public float camShakeMultiplier = 0.1f;
    private GameObject player;

    void CameraShake(bool activate)
    {
        angle = 360 * Random.value;
        camX = camShakeMultiplier * Mathf.Cos(angle);
        camY = camShakeMultiplier * Mathf.Sin(angle);

        //if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("stand"))
        //{
            if (activate == true)
            {
                transform.position = new Vector3(
                    transform.position.x + camX,
                    transform.position.y + camY,
                    -10);
            }
            else
            {
                transform.position = new Vector3(
                    initialCamX,
                    initialCamY,
                    -10);
            }
        //}
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

   
    void Update()
    {
        if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hurt"))
        {
            CameraShake(true);
        }
        else
        {
            CameraShake(false);
        }
    }
}
