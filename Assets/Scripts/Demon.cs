using UnityEngine;

public class Demon : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;
    private GameObject player;
    private BoxCollider2D boxCollider;

    public GameObject demonProjectile;

    double realTime;
    double standResetTime;

    bool standReset;

    float velocityX;
    float velocityY;
    public float standMovementIncrementRatio = 2;
    public float standMovementVelocity = 2;

    bool shotReset;
    bool shotAnimReset;

    public Vector3 startingPos = new Vector3(5.2f, 0, -1);

    private float randomValue;

    void Start()
    {
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

        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("stand") && !standReset)
        //{
        //    standReset = true;
        //}
        //else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("stand") && standReset)
        //{
        //    standReset = false;
        //}


        // shot attack sequence
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("shot"))
        {
            body.linearVelocity = new Vector2(0, 0);

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

            if (transform.position.x <= -8.5f)
            {
                body.linearVelocity = new Vector2(0, 0);
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
            animator.SetBool("teleportIn", true);
            animator.SetBool("kick2", false);
        }


        // fly attack sequence
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("flyBack"))
        {
            if (transform.position.x >= 15)
            {
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
    }
}
