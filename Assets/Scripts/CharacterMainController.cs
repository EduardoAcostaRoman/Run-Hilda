using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public AudioClip audioJump;
    public AudioClip audioFall;
    public AudioClip audioHurt1;
    public AudioClip audioHurt2;
    public AudioClip audioHurt3;
    public AudioClip audioHurt4;

    public Shader shaderGUItext;
    public Shader shaderSpritesDefault;
    private SpriteRenderer sprites;

    float realtime;
    float prevtime;
    bool colorChange = false;
    int colorCount;
    public bool blink = false;

    public GameObject dust;
    private GameObject speedReference;

    public GameObject hitEffect;

    private float randomValue;

    void clearSprite()
    {
        sprites.material.shader = shaderSpritesDefault;
        sprites.color = Color.clear;
    }

    void whiteSprite()
    {
        sprites.material.shader = shaderGUItext;
        sprites.color = Color.white;
    }

    void normalSprite()
    {
        sprites.material.shader = shaderSpritesDefault;
        sprites.color = Color.white;
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

        realtime = Time.fixedTime;
        randomValue = Random.value;

        // For PC tests (delete/comment for build)

        if (Input.GetKeyDown(KeyCode.W) && boxCollider.IsTouchingLayers(groundLayer) && animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            audioPlayer(audioJump, 0.5f);
            body.velocity = new Vector2(0, jumpForce);       
        }

        // For PC tests (delete/comment for build)

        // --- MOVEMENT --- //

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && boxCollider.IsTouchingLayers(groundLayer))
            {
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
            if (animator.GetBool("fall") == true)
            {
                audioPlayer(audioFall, 0.5f);
                dust.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                dust.transform.position = new Vector3(transform.position.x -0.45f, dust.transform.position.y, dust.transform.position.z);
                dust.GetComponent<Animator>().SetBool("dust", true);
            }
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

        if (transform.GetChild(0).GetComponent<BoxCollider2D>().IsTouchingLayers(enemyLayer) && !blink)
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
                if (!colorChange)
                {
                    if (colorCount < 4)
                    {
                        whiteSprite();
                    }
                    else
                    {
                        clearSprite();
                    }                 
                }
                else
                {
                    normalSprite();
                }

                colorChange = !colorChange;

                colorCount += 1;

                prevtime = realtime;
            }

            if (colorCount >= invicibilityTime)
            {
                sprites.color = new Color(255, 255, 255, 255);
                colorCount = 0;
                normalSprite();
                blink = false;
            }
        }
        else
        {
            colorChange = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("hurt") && animator.GetBool("hurt"))
        {
            animator.SetBool("hurt", false);
        }

    }
}
