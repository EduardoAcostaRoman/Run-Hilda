using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class PuffBall : MonoBehaviour
{
    private Rigidbody2D body;
    
    private GameObject target;
    private bool targetAquired;

    public float setVelocity = 5f;
    public float setVelocitySmoothRate = 0.3f;

    float pastPosX = 20;

    private Vector2 velocity = Vector2.zero; // Used by SmoothDamp

    private void lookForEnemies()
    {
        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemiesInScene)
        {
            float currentPosX = enemy.transform.position.x;

            if (currentPosX < pastPosX && currentPosX >= -3)   // if the enemy is the closest and didn't pass the player then it
            {                                                  // becomes the target
                target = enemy;
            }

            pastPosX = enemy.transform.position.x;
        }
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Enemy chaser
        if (target)
        {
            if (transform.position.x < target.transform.position.x)
            {
                // Calculate the desired velocity toward the target
                Vector2 targetDirection = (target.transform.position - transform.position).normalized;
                Vector2 targetVelocity = targetDirection * setVelocity;

                // Smoothly interpolate the current velocity toward the target velocity
                body.linearVelocity = Vector2.SmoothDamp(body.linearVelocity, targetVelocity, ref velocity, setVelocitySmoothRate);

                //transform.position = Vector2.SmoothDamp(transform.position, target.transform.position, ref velocity, setVelocity);
            }

            targetAquired = true;
        }
        else if (!targetAquired)
        {
            lookForEnemies();
        }
    }
}
