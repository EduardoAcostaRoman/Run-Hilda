using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Demon : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;
    private GameObject player;
    private BoxCollider2D boxCollider;

    public Shader shaderGUItext;
    public Shader shaderSpritesDefault;
    SpriteRenderer sprite;

    public LayerMask playerProjectileLayer;

    public GameObject demonProjectile;

    double realTime;
    double standResetTime;

    float velocityX;
    float velocityY;
    public float standMovementIncrementRatio = 2;
    public float standMovementVelocity = 2;

    bool shotReset;
    bool shotAnimReset;

    bool flySoundReset;
    bool teleportSoundReset;
    bool shotSoundReset;
    bool finalShotSoundReset;
    bool shotPhraseReset;

    int shotSoundCount = 6;

    public Vector3 startingPos = new Vector3(5.2f, 0, -1);

    private float randomValue;

    bool blink;
    int blinkCount = 1;
    double blinkTimeReset;

    int damage = 0;
    public int deathVal = 10;
    double explosionTimeReset;
    public GameObject explosion;
    public GameObject finalExplosion;
    public float explosionDistance = 2;
    public float explosionOffset = 1;
    int explosionCount;
    int explosionCountForfinalExplosion = 8;

    private void OnTriggerEnter2D(Collider2D collisionObject)
    {

        // For projectile collision
        if (collisionObject.tag == "PlayerProjectile")
        {
            // to change colors when hurt to white
            blink = true;
        }
    }

    void PlayAudio(string sprite, ref bool resetVar, int audioSourceObjectNum)
    {
        if (GetComponent<SpriteRenderer>().sprite.name == sprite && !resetVar)
        {
            transform.GetChild(audioSourceObjectNum).GetComponent<AudioSource>().Play();
            resetVar = true;
        }
        else if (GetComponent<SpriteRenderer>().sprite.name != sprite)
        {
            resetVar = false;
        }
    }

    void whiteSprite()
    {
        sprite.material.shader = shaderGUItext;
        sprite.color = Color.white;
    }

    void normalSprite()
    {
        sprite.material.shader = shaderSpritesDefault;
        sprite.color = Color.white;
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        transform.position = startingPos;
    }

    void Update()
    {
        realTime = Time.timeSinceLevelLoad;
        randomValue = Random.value;

        //base stand animation movement in a infinite character shape (lemniscate)
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("stand"))
        {
            // wing flip auido play
            PlayAudio("stand_4", ref flySoundReset, 4);

            velocityX = standMovementIncrementRatio * Mathf.Sin((float)realTime * standMovementVelocity);
            velocityY = standMovementIncrementRatio * Mathf.Cos(2 * ((float)realTime * standMovementVelocity));

            body.linearVelocity = new Vector2(velocityX, velocityY);
        }



        // go to stand position and proceed to attack every X seconds
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("stand") && (realTime - standResetTime >= 3))
        {
            if (randomValue < 0.33f)
            {
                animator.SetBool("flyBack", true);
            }
            else if (randomValue < 0.66f)
            {
                animator.SetBool("kick1", true);
            }
            else
            {
                animator.SetBool("shot", true);
            }

            standResetTime = realTime;
        }
        
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("stand"))
        {
            standResetTime = realTime;
        }


        // shot attack sequence
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("shot"))
        {
            if (!shotPhraseReset)
            {
                transform.GetChild(10).GetComponent<AudioSource>().Play();
                shotPhraseReset = true;
            }

            body.linearVelocity = new Vector2(0, 0);

            // charge sound
            if (GetComponent<SpriteRenderer>().sprite.name == "shot_1" && !shotSoundReset)
            {
                transform.GetChild(shotSoundCount).GetComponent<AudioSource>().Play();

                shotSoundCount++;
                shotSoundCount = Mathf.Clamp(shotSoundCount, 6, 8); //to just make the 3 charge sounds

                shotSoundReset = true;
            }
            else if (GetComponent<SpriteRenderer>().sprite.name != "shot_1")
            {
                shotSoundReset = false;
            }

            // final shot sound
            PlayAudio("shot_7", ref finalShotSoundReset, 9);

            // to shoot 3 shots
            if (GetComponent<SpriteRenderer>().sprite.name == "blank" && !shotAnimReset)
            {
                if (transform.position.y > -1.5f)
                {
                    transform.position = new Vector3(7f, -1.5f, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(7f, 0.5f, transform.position.z);
                }

                shotAnimReset = true;
            }
            else if (GetComponent<SpriteRenderer>().sprite.name != "blank")
            {
                shotAnimReset = false;
            }

            if (GetComponent<SpriteRenderer>().sprite.name == "shot_7" && !shotReset)
            {
                Instantiate(demonProjectile, new Vector3(transform.position.x,
                                                         transform.position.y + 0.4f, 
                                                         demonProjectile.transform.position.z), 
                                                         demonProjectile.transform.rotation);
                shotReset = true;

            }
            else if (GetComponent<SpriteRenderer>().sprite.name != "shot_7")
            {
                shotReset = false;
            }

            animator.SetBool("shot", false);
            animator.SetBool("teleportIn", true);
        }
        else
        {
            shotPhraseReset = false;
            shotSoundCount = 6;
            shotAnimReset = false;
        }


        // kick attack sequence
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("kick"))
        {
            body.linearVelocity = new Vector2(0, 0);

            if (GetComponent<SpriteRenderer>().sprite.name == "blank")
            {
                transform.position = new Vector3(-1, -1.5f, transform.position.z);
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("kick2"))
        {
            transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;

            if (body.linearVelocity.x != -30)
            {
                transform.GetChild(0).GetComponent<AudioSource>().Play();
                transform.GetChild(5).GetComponent<AudioSource>().Play();
            }

            if (transform.position.x <= -8.5f)
            {
                animator.SetBool("kick2", true);
                animator.SetBool("kick1", false);
            }
            else
            {
                body.linearVelocity = new Vector2(-30, 0);
            }
        }
        else
        {
            transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("kick3"))
        {
            body.linearVelocity = new Vector2(0, 0);
            animator.SetBool("teleportIn", true);
            animator.SetBool("kick2", false);
        }


        // fly attack sequence
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("flyBack"))
        {
            if (body.linearVelocity.x != 10)
            {
                transform.GetChild(2).GetComponent<AudioSource>().Play();
            }

            if (transform.position.x >= 15)
            {
                transform.GetChild(1).GetComponent<AudioSource>().Play(); 

                body.linearVelocity = new Vector2(0, 0);
                transform.position = new Vector3(transform.position.x, player.transform.position.y + 2, transform.position.z);
                animator.SetBool("flyAttack", true);
                animator.SetBool("flyBack", false);
            }
            else
            {
                body.linearVelocity = new Vector2(10, 0);
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("flyAttack"))
        {
            transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = true;

            if (transform.position.x <= -20)
            {
                animator.SetBool("teleportIn", true);
                animator.SetBool("flyAttack", false);
            }
            else
            {
                body.linearVelocity = new Vector2(-25, 0);
            }
        }
        else
        {
            transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = false;
        }


        // teleporting back to stand position everytime "teleportIn" is called in animator
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("teleportIn"))
        {
            body.linearVelocity = new Vector2(0, 0);
            transform.position = startingPos;
            animator.SetBool("teleportIn", false);
        }


        // teleport sound
        PlayAudio("blank", ref teleportSoundReset, 3);


        // damage managing
        if (blink && (realTime - blinkTimeReset >= 0.1f))
        {
            if (blinkCount == 1)
            {
                transform.GetChild(11).GetComponent<AudioSource>().Play();
            }

            if (blinkCount % 2 == 0)
            {
                normalSprite();
            }
            else
            {
                whiteSprite();
            }

            blinkCount++;
            blinkTimeReset = realTime;

            if (blinkCount >= 5)
            {
                damage++;
                explosionTimeReset = realTime;
                blink = false;
                blinkCount = 1;
                normalSprite();
            }
        }

        if (damage >= deathVal && explosionCount < explosionCountForfinalExplosion)
        {
            if (!animator.GetBool("death"))
            {
                transform.GetChild(12).GetComponent<AudioSource>().Play();
            }

            body.linearVelocity = new Vector2(0, 0);
            animator.SetBool("death", true);

            if (realTime - explosionTimeReset >= 0.4f)
            {
                Instantiate(explosion, new Vector3(transform.position.x + (randomValue * explosionDistance) - explosionOffset,
                                                   transform.position.y + (Random.value * explosionDistance) - explosionOffset,
                                                   -1.1f),
                                                   explosion.transform.rotation);

                explosionCount++;
                explosionTimeReset = realTime;
            }
        }

        if (explosionCount >= explosionCountForfinalExplosion)
        {
            Instantiate(finalExplosion, new Vector3(transform.position.x,
                                                   transform.position.y,
                                                   -1.1f),
                                                   finalExplosion.transform.rotation);

            Destroy(gameObject);
        }
    }
}
