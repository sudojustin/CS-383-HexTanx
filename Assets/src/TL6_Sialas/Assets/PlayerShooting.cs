using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; 

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click to shoot
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Get mouse position and convert to world coordinates
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        // Instantiate projectile at player position
        GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Move projectile toward target 
        Projectile projectileScript = bullet.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetTarget(mousePosition);
        }
    }
}
