using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private AudioClip shootSoundOverride; // Optional override for shoot sound
    [SerializeField] private float angleOffset = 0f; // Adjust in Inspector to fix barrel alignment
    [SerializeField] private Transform barrelTip; // Reference to the BarrelTip Transform
    private Camera mainCamera;
    private PlayerTank playerTank;
    private PlaceTile placeTile;

    void Start()
    {
        mainCamera = Camera.main;
        // Get PlayerTank from the parent GameObject
        playerTank = GetComponentInParent<PlayerTank>();
        if (playerTank == null)
        {
            Debug.LogError("PlayerTank component not found in parent!");
        }
        // Get PlaceTile for grid access
        placeTile = FindObjectOfType<PlaceTile>();
        if (placeTile == null)
        {
            Debug.LogError("PlaceTile not found in scene!");
        }
        // Find the BarrelTip child
        if (barrelTip == null)
        {
            barrelTip = transform.Find("BarrelTip");
            if (barrelTip == null)
            {
                Debug.LogError("BarrelTip not found! Please add a BarrelTip child to the Turret.");
            }
        }
    }

    void Update()
    {
        // Rotate the turret to face the mouse
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // Ensure Z matches the turret's Z (2D game)

        // Calculate the direction from the turret to the mouse
        Vector3 direction = mouseWorldPos - transform.position;
        direction.z = 0; // Ignore Z for 2D rotation

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;

        // Rotate the turret to face the mouse
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Right-click to shoot
        if (Input.GetMouseButtonDown(1))
        {
            // Check if there are enough action points and ammo before proceeding
            if (playerTank.GetActionPoints() <= 0)
            {
                Debug.Log("No action points remaining!");
                return;
            }
            if (playerTank.GetAmmoCount() <= 0)
            {
                Debug.Log("No ammo remaining!");
                return;
            }

            // Get the nearest tile to the mouse position
            Vector3 targetTilePos = FindNearestTile(mouseWorldPos);
            if (targetTilePos != Vector3.zero) // Check if a valid tile was found
            {
                // Deduct action point only if a valid tile is found
                if (playerTank.UseActionPoint())
                {
                    Shoot(targetTilePos);
                    playerTank.SetAmmoCount(playerTank.GetAmmoCount() - 1);  // Deduct ammo
                    Debug.Log("Shot fired toward tile center: " + targetTilePos);
                    FindObjectOfType<BattleSystem>().PlayerActionTaken();
                }
            }
            else
            {
                Debug.Log("Mouse position is not over a valid tile - shot cancelled.");
            }
        }
    }

    void Shoot(Vector3 targetPosition)
    {
        // Play shoot sound via SoundManager
        if (shootSoundOverride != null)
        {
            //SoundManager.Instance.Play(shootSoundOverride); // Use override if assigned
        }
        else
        {
            //SoundManager.Instance.ShootSound(); // Use default from SoundManager
        }
        Debug.Log("Shoot sound triggered via SoundManager");

        // Instantiate projectile at barrel tip position and turret rotation
        if (barrelTip != null)
        {
            GameObject bullet = Instantiate(projectilePrefab, barrelTip.position, transform.rotation);

            // Move projectile toward target
            Projectile projectileScript = bullet.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetTarget(targetPosition);
            }
        }
        else
        {
            Debug.LogWarning("BarrelTip is null - spawning projectile at turret position instead.");
            GameObject bullet = Instantiate(projectilePrefab, transform.position, transform.rotation);
            Projectile projectileScript = bullet.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetTarget(targetPosition);
            }
        }
    }

    private Vector3 FindNearestTile(Vector3 position)
    {
        if (placeTile == null || placeTile.Grid == null)
        {
            Debug.LogError("PlaceTile or Grid not available");
            return Vector3.zero; // Return invalid position if grid isn’t available
        }

        // Find the nearest tile to the mouse position
        Vector3 nearestPos = placeTile.Grid[0, 0];
        float minDistance = Vector3.Distance(position, nearestPos);

        for (int x = 0; x < placeTile.Grid.GetLength(0); x++)
        {
            for (int y = 0; y < placeTile.Grid.GetLength(1); y++)
            {
                float distance = Vector3.Distance(position, placeTile.Grid[x, y]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPos = placeTile.Grid[x, y];
                }
            }
        }

        // Check if the mouse is over a valid tile (e.g., within a certain distance threshold)
        float maxTileDistance = 0.5f; 
        if (minDistance <= maxTileDistance)
        {
            // Return the tile center
            return new Vector3(nearestPos.x, nearestPos.y, -1); 
        }
        else
        {
            // Return invalid position if mouse isn’t over a tile
            return Vector3.zero; 
        }
    }
}