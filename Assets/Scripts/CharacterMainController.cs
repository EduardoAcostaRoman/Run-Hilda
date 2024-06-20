using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMainController : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private AudioSource audioSource;

    public int invicibilityTime = 16;
    public float jumpForce = 20;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public LayerMask itemLayer;

    public AudioClip audioJump;
    public AudioClip audioFall;
    public AudioClip audioHurt1;
    public AudioClip audioHurt2;
    public AudioClip audioHurt3;
    public AudioClip audioHurt4;
    public AudioSource audioDeath2;
    public AudioClip audioDeath2Effect;

    public Shader shaderGUItext;
    public Shader shaderSpritesDefault;
    private SpriteRenderer sprites;

    double realtime;
    double prevtime;
    double deathWaitTime;

    bool colorChange = false;
    int colorCount;
    public bool blink = false;

    public GameObject dust;
    private GameObject speedReference;

    public GameObject hitEffect;

    private float randomValue;

    private double prevTimeDeath2Effect;
    private int death2EffectCounter;
    public GameObject death2Effect;
    public float death2EffectDistance = 0.1f;
    public float death2EffectAngle = 0;

    private bool buffTrigger = false;
    public float shieldColor = 4;
    private bool shieldActivated = false;

    // --- ITEMS ---//

    private void OnTriggerEnter2D(Collider2D collisionObject)
    {

        // Buff item 
        if (collisionObject.tag == "Buff" && !animator.GetBool("death"))
        {
            buffTrigger = true;
            transform.GetChild(3).GetComponent<AudioSource>().Play();
            Destroy(collisionObject.gameObject);
        }
    }

    void audioPlayer(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        sprites = GetComponent<SpriteRenderer>();

        speedReference = GameObject.FindGameObjectWithTag("Background");

        Physics2D.IgnoreLayerCollision(6, 7); // Ignores collision between layers: (6 = player || 7 = enemy) 
        Physics2D.IgnoreLayerCollision(6, 8); // Ignores collision between layers: (6 = player || 8 = intangible) 
    }

    void Update()
    {

        // --- CONFIGURATIONS --- //

        realtime = Time.timeSinceLevelLoad;
        randomValue = Random.value;

        // For PC tests (delete/comment for build)

        if (Input.GetKeyDown(KeyCode.W) && boxCollider.IsTouchingLayers(groundLayer) && animator.GetCurrentAnimatorStateInfo(0).IsName("run") && Time.timeScale == 1f && !animator.GetBool("death"))
        {
            audioPlayer(audioJump, 0.5f);
            body.velocity = new Vector2(0, jumpForce);       
        }

        // For PC tests (delete/comment for build)

        // --- MOVEMENT --- //

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && boxCollider.IsTouchingLayers(groundLayer) && animator.GetCurrentAnimatorStateInfo(0).IsName("run") && Time.timeScale == 1f && !animator.GetBool("death"))
            {
                // if its touching UI canvas, player is not going to jump
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;         
                audioPlayer(audioJump, 0.5f);
                body.velocity = new Vector2(0, jumpForce);
            }
        }

        if (!boxCollider.IsTouchingLayers(groundLayer) && (animator.GetCurrentAnimatorStateInfo(0).IsName("jump") || animator.GetCurrentAnimatorStateInfo(0).IsName("hurt")))
        {
            animator.SetBool("jump", false);
        }

        if (boxCollider.IsTouchingLayers(groundLayer))
        {
            if (animator.GetBool("fall"))
            {
                audioPlayer(audioFall, 0.5f);
                dust.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                dust.transform.position = new Vector3(transform.position.x -0.45f, dust.transform.position.y, dust.transform.position.z);
                dust.GetComponent<Animator>().SetBool("dust", true);
            }
            animator.SetBool("jump", false);
            animator.SetBool("fall", false);
        }
        else
        {
            if (body.velocity.y <= 0)
            {
                animator.SetBool("fall", true);
            }
            else if (body.velocity.y > 0)
            {
                animator.SetBool("jump", true);
            }
        }

        if (dust.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("dust"))
        {
            dust.GetComponent<Rigidbody2D>().velocity = new Vector2((speedReference.GetComponent<Animator>().speed + 0.3f) * -8, 0);
            dust.GetComponent<Animator>().SetBool("dust", false);
        }

        // --- DAMAGES ---//

        if (transform.GetChild(0).GetComponent<BoxCollider2D>().IsTouchingLayers(enemyLayer) && !blink && !animator.GetBool("death"))
        {
            animator.SetBool("hurt", true);

            if (randomValue < 0.25)
            {
                audioPlayer(audioHurt1, 1f);
            }
            else if (randomValue < 0.5)
            {
                audioPlayer(audioHurt2, 1f);
            }
            else if (randomValue < 0.75)
            {
                audioPlayer(audioHurt3, 1f);
            }
            else
            {
                audioPlayer(audioHurt4, 0.7f);
            }

            Instantiate(hitEffect, new Vector3(transform.position.x,transform.position.y +1.08f, -1f), hitEffect.transform.rotation);

            prevtime = realtime;
            blink = true;
        }

        if (blink)
        {
            if (realtime - prevtime > 0.08f)
            {
                if (!colorChange && !animator.GetCurrentAnimatorStateInfo(0).IsName("hurt"))
                {
                    sprites.color = Color.clear;
                }
                else
                {
                    sprites.color = Color.white;
                }

                colorChange = !colorChange;

                colorCount += 1;

                prevtime = realtime;
            }

            if (colorCount >= invicibilityTime)
            {
                colorCount = 0;
                sprites.color = Color.white;
                blink = false;
            }

            // if dead, make player not move
            if (animator.GetBool("death"))
            {
                body.velocity = new Vector2(0, 0);
                body.gravityScale = 0;
            }
        }
        else
        {
            colorChange = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("hurt") && animator.GetBool("hurt"))
        {
            //if no shield up, then character dies
            if (!shieldActivated)
            {
                animator.SetBool("death", true);

                // player death notification
                NotificationCenter.DefaultCenter().PostNotification(this, "PlayerDead");
            }
            
            buffTrigger = false;
            animator.SetBool("hurt", false);
        }

        // --- CHARACTER DEATH ---//


        // death animation 1 (explosion)

        // death animation 2 (megaman-like)

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("death2"))
        {
            body.velocity = new Vector2(0, 0);
            body.gravityScale = 0;

            if (realtime - prevTimeDeath2Effect > 0.7 && death2EffectCounter < 2)
            {
                if (death2EffectCounter == 0)
                {
                    audioPlayer(audioDeath2Effect, 0.5f);
                    audioDeath2.Play();
                }

                for (int i = 1; i < 9; i++)
                {
                    //GameObject effect = Instantiate(death2Effect, new Vector3(transform.position.x + (death2EffectDistance * Mathf.Sin(death2EffectAngle)),
                    //    transform.position.y + 1.08f + (death2EffectDistance * Mathf.Cos(death2EffectAngle)),
                    //    transform.position.z),
                    //death2Effect.transform.rotation);

                    //effect.transform.GetChild(0).transform.position = new Vector2(death2EffectAngle, 0);

                    //death2EffectAngle += 0.785f; // 1.57 is 90° rotation, then 0.785 is 45°, which makes effect spawn in 8 ways

                    GameObject effect = Instantiate(death2Effect, new Vector3(transform.position.x,
                        transform.position.y + 1.08f,
                        transform.position.z),
                        death2Effect.transform.rotation);

                    death2EffectAngle = i;

                    effect.transform.GetChild(0).transform.position = new Vector2(death2EffectAngle, 0);
                }

                death2EffectCounter += 1;
                prevTimeDeath2Effect = realtime;
            }
        }


        // --- ITEMS ---//


        // Buff

        if (buffTrigger && !animator.GetBool("death"))
        {
            shieldActivated = true;
            transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 
                Mathf.Lerp(transform.GetChild(2).GetComponent<SpriteRenderer>().color.a, shieldColor, 0.1f));

            NotificationCenter.DefaultCenter().PostNotification(this, "BuffActivated");
        }
        else
        {
            shieldActivated = false;
            transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,
                Mathf.Lerp(transform.GetChild(2).GetComponent<SpriteRenderer>().color.a, 0, 0.1f));

            NotificationCenter.DefaultCenter().PostNotification(this, "BuffNotActivated");
        }
    }
}
