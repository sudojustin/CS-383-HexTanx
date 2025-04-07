using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed = 5f;
    public int damage = 10;
    [SerializeField] private AudioClip explosionSound;

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    void Update()
    {
        // Move projectile towards target
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Destroy projectile when it reaches target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
    public void SetDamage(int damageValue)
    {
        damage = damageValue;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerTank playerTank = other.GetComponent<PlayerTank>();

        if (playerTank != null)
        {
            playerTank.SetHealth(playerTank.GetHealth() - damage);
            //Debug.Log("Player tank hit! Remaining HP: " + playerTank.GetHealth());

            if (playerTank.GetHealth() <= 0)
            {
                if (explosionSound != null)
                {
                    AudioSource.PlayClipAtPoint(explosionSound, transform.position);
                    //Debug.Log("Explosion sound triggered at position: " + transform.position);
                }
                else
                {
                    //Debug.LogWarning("Explosion sound not played: AudioClip missing!");
                }
                Destroy(playerTank.gameObject);
                //Debug.Log("Player tank destroyed!");
            }
            Destroy(gameObject);
        }
    }
}