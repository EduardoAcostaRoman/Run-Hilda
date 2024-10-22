using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PuffBall : MonoBehaviour
{
    bool targetAquired = false;
    private GameObject target;

    public float setVelocityY = 5;

    private void OnTriggerEnter2D(Collider2D collisionObject)
    {

        // For enemy collision
        if (collisionObject.tag == "Enemy" && !targetAquired)
        {
            target = collisionObject.gameObject;
            targetAquired = true;  
        }
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        // Enemy chaser
        //if (targetAquired && target && (transform.parent.position.x <= target.transform.position.x))
        //{
        //    transform.parent.GetComponent<Rigidbody2D>().linearVelocityX = Mathf.Lerp(transform.parent.GetComponent<Rigidbody2D>().linearVelocityX, target.GetComponent<Rigidbody2D>().linearVelocityX, Time.deltaTime);
        //    if (transform.parent.position.y <= target.transform.position.y)
        //    {
        //        transform.parent.GetComponent<Rigidbody2D>().linearVelocityY = setVelocityY;
        //    }
        //    else
        //    {
        //        transform.parent.GetComponent<Rigidbody2D>().linearVelocityY = -setVelocityY;
        //    }
        //}
    }
}
