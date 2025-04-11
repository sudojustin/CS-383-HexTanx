using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject playerTank;
    private PlayerTank playerTankComponent;
    private PlaceTile placeTile;

    private Vector3 targetPosition; // The tile position to move towards
    private Vector2Int currentGridPos; // Current position in grid coordinates (x, y)
    private bool isMoving = false;  // Flag to track if the tank is currently moving
    [SerializeField] private float moveSpeed = 5f; // Speed of the tank movement (adjust in Inspector)

    void Start()
    {
        mainCamera = Camera.main;
        playerTank = gameObject;
        playerTankComponent = playerTank.GetComponent<PlayerTank>();
        placeTile = FindObjectOfType<PlaceTile>();
        targetPosition = transform.position; // Initialize target to current position

        // Initialize current grid position
        currentGridPos = WorldToGridPosition(transform.position);
    }

    void Update()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        // Handle movement towards the target position
        if (isMoving)
        {
            MoveToTarget();
        }

        // Check if the player has action points
        if (playerTankComponent.GetActionPoints() <= 0)
        {
            FindObjectOfType<BattleSystem>().PlayerActionTaken();
            return;
        }

        // Handle click and move logic
        HandleClickAndMove();
    }

    // New function to handle all click-and-move logic
    private void HandleClickAndMove()
    {
        // Left-click to initiate movement
        if (Input.GetMouseButtonDown(0) && !isMoving) // Only allow new movement if not currently moving
        {
            if (playerTankComponent.GetActionPoints() <= 0)
            {
                Debug.Log("No action points remaining!");
                return;
            }

            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = -1;
            Vector3 potentialTarget = FindNearestTile(mouseWorldPos);

            // Check if the target tile is within range (one tile away in hex grid)
            Vector2Int targetGridPos = WorldToGridPosition(potentialTarget);
            if (IsWithinRange(currentGridPos, targetGridPos))
            {
                // Check if the target tile is an EarthTerrain tile
                if (IsEarthTerrain(potentialTarget))
                {
                    Debug.Log("Cannot move to target tile: it is an EarthTerrain (mountain) tile!");
                    return; // Prevent movement to EarthTerrain tiles
                }
                if (playerTankComponent.UseActionPoint()) // Check and deduct action point
                {
                    targetPosition = potentialTarget;
                    if (targetPosition != transform.position) // Only move if the target is different
                    {
                        isMoving = true; // Start moving
                        SoundManager.GetInstance().PlayerMoveSound(); // Play movement sound
                        //Debug.Log("Tank moving to " + targetPosition);
                        //Debug.Log("Mouse position " + mouseWorldPos);
                        FindObjectOfType<BattleSystem>().PlayerActionTaken();
                    }
                }
            }
            else
            {
                Debug.Log("Target tile is out of range (must be one tile away in hex grid)!");
            }
        }
    }

    // public method to allow tests to initiate movement directly
    public void SetTargetAndMove(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
        if (targetPosition != transform.position) // Only move if the target is different
        {
            isMoving = true; // Start moving
            SoundManager.GetInstance().PlayerMoveSound(); // Play movement sound
            FindObjectOfType<BattleSystem>().PlayerActionTaken();
        }
    }

    // Function to handle the actual movement functionality
    private void MoveToTarget()
    {
        // Move towards the target position at moveSpeed
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the tank has reached the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            transform.position = targetPosition; // Snap to target to avoid floating-point issues
            isMoving = false; // Stop moving
            playerTankComponent.SetTankLocation(targetPosition); // Update PlayerTank's location
            SoundManager.GetInstance().StopMovementSound(); // Stop the movement sound
            //Debug.Log("Tank reached target: " + transform.position);

            // Update current grid position
            currentGridPos = WorldToGridPosition(targetPosition);
        }
    }

    public bool IsWithinRange(Vector2Int currentPos, Vector2Int targetPos)
    {
        // For a hex grid with odd-row offset (odd rows shifted right)
        int dx = targetPos.x - currentPos.x;
        int dy = targetPos.y - currentPos.y;

        if (currentPos.y % 2 == 0) // Even row
        {
            bool isAdjacent = (dx == 0 && dy == -1) || // Up
                              (dx == 0 && dy == 1) ||  // Down
                              (dx == -1 && dy == -1) || // Left-Up
                              (dx == -1 && dy == 1) ||  // Left-Down
                              (dx == -1 && dy == 0) ||  // Left
                              (dx == 1 && dy == 0);     // Right
            return isAdjacent;
        }
        else // Odd row
        {
            bool isAdjacent = (dx == 0 && dy == -1) || // Up
                              (dx == 0 && dy == 1) ||  // Down
                              (dx == -1 && dy == 0) || // Left
                              (dx == 1 && dy == 0) ||  // Right
                              (dx == 1 && dy == -1) || // Right-Up
                              (dx == 1 && dy == 1);    // Right-Down
            return isAdjacent;
        }
    }

    public bool IsEarthTerrain(Vector3 position)
    {
        Collider2D tileCollider = Physics2D.OverlapPoint(position);
        if (tileCollider != null)
        {
            EarthTerrain terrain = tileCollider.GetComponent<EarthTerrain>();
            if (terrain != null)
            {
                Debug.Log(gameObject.name + " avoiding Earth Terrain at: " + position);
                return true;
            }
        }
        return false;
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPos)
    {
        if (placeTile == null || placeTile.Grid == null)
        {
            Debug.LogError("PlaceTile or Grid not available");
            return Vector2Int.zero;
        }

        // Find the closest grid position
        Vector2Int gridPos = Vector2Int.zero;
        float minDistance = float.MaxValue;

        for (int x = 0; x < placeTile.Grid.GetLength(0); x++)
        {
            for (int y = 0; y < placeTile.Grid.GetLength(1); y++)
            {
                float distance = Vector3.Distance(worldPos, placeTile.Grid[x, y]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    gridPos = new Vector2Int(x, y);
                }
            }
        }

        return gridPos;
    }

    public Vector3 FindNearestTile(Vector3 position)
    {
        if (placeTile == null || placeTile.Grid == null)
        {
            Debug.LogError("PlaceTile or Grid not available");
            return transform.position;
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
}