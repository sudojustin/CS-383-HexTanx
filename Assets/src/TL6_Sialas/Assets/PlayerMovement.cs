using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject playerTank;
    private PlayerTank playerTankComponent;  // Reference to PlayerTank script
    private PlaceTile placeTile;

    [SerializeField] private GameObject projectilePrefab;  // Assign in Inspector
    [SerializeField] private Transform firePoint;          // Where projectile spawns (e.g., tank barrel)

    void Start()
    {
        mainCamera = Camera.main;
        playerTank = gameObject;
        playerTankComponent = playerTank.GetComponent<PlayerTank>();
        placeTile = FindObjectOfType<PlaceTile>();
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("Projectile Prefab or Fire Point not assigned in PlayerMovement!");
        }
    }

    void Update()
    {
        // Left-click to move
        if (Input.GetMouseButtonDown(0))
        {
            if (playerTankComponent.UseActionPoint())  // Check and deduct action point
            {
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPos.z = -1;
                Vector3 newPosition = FindNearestTile(mouseWorldPos);
                playerTankComponent.SetTankLocation(newPosition);  // Use setter for consistency
                Debug.Log("Tank moved to " + playerTank.transform.position);
                Debug.Log("Mouse position " + mouseWorldPos);
                FindObjectOfType<BattleSystem>().PlayerActionTaken();
            }
        }
    }

    public Vector3 FindNearestTile(Vector3 position)
    {
        if (placeTile == null || placeTile.Grid == null)
        {
            Debug.LogError("PlaceTile or Grid not available");
            return playerTank.transform.position;
        }

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

        return new Vector3(nearestPos.x, nearestPos.y, -1);
    }

    private void Shoot(Vector3 targetPosition)
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetTarget(targetPosition);
            }
        }
    }
}