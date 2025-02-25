using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed = 5f; 
    public int damage = 10; 

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    void Update()
    {
        // shoot shell towards target
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Destroy projectile when it reaches target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the shell hit enemy tank
        TankType enemyTank = other.GetComponent<TankType>();

        if (enemyTank != null)
        {
            // Apply damage
            enemyTank.health -= damage; 
            Debug.Log("Enemy tank hit! Remaining HP: " + enemyTank.health);

            if (enemyTank.health <= 0)
            {
                Destroy(enemyTank.gameObject);
                Debug.Log("Enemy tank destroyed!");
            }

            Destroy(gameObject); 
        }
    }
}

