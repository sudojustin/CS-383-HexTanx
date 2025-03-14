using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject playerTank;
    private PlaceTile placeTile;

    void Start()
    {
        mainCamera = Camera.main;
        playerTank = gameObject;
        placeTile = FindObjectOfType<PlaceTile>();  
    }

    void Update()
    {
        // Left-click
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = -1; 
            playerTank.transform.position = FindNearestTile(mouseWorldPos);
            Debug.Log("Tank moved to " + playerTank.transform.position);
        }
    }

    Vector3 FindNearestTile(Vector3 position)
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
}