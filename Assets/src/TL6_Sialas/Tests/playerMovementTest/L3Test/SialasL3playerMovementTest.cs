using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class PlayerTankMovementTest
{
    private GameObject playerTank;
    private PlayerMovement playerMovement;
    private Vector3 initialPosition;

    [UnitySetUp]
    public IEnumerator LoadScene()
    {
        SceneManager.LoadScene("level3");
        yield return new WaitForSeconds(1.0f); // Ensure scene is loaded before continuing
    }

    [UnityTest]
    public IEnumerator PlayerTankMovementTestLevel3()
    {
        // Find the player tank
        playerTank = GameObject.Find("PlayerTank");
        Assert.NotNull(playerTank, "Player tank not found in the scene!");

        playerMovement = playerTank.GetComponent<PlayerMovement>();
        Assert.NotNull(playerMovement, "PlayerMovement component not found on player tank!");

        // Get PlaceTile for tile validation
        PlaceTile placeTile = GameObject.FindObjectOfType<PlaceTile>();
        Assert.NotNull(placeTile, "PlaceTile not found in the scene!");
        Debug.Log($"PlaceTile found with grid size: {placeTile.width}x{placeTile.height}");

        // Store initial position
        initialPosition = playerTank.transform.position;
        yield return new WaitForSeconds(0.5f); // Allow initialization
        Debug.Log($"Initial position: {initialPosition}");

        // Simulate 64 moves
        int moves = 0;
        const int maxMoves = 20;
        Vector3 previousPosition = initialPosition;
        Vector2Int currentGridPos = playerMovement.WorldToGridPosition(initialPosition);

        while (moves < maxMoves)
        {
            // Define possible adjacent positions in grid coordinates (same as IsWithinRange logic)
            Vector2Int[] possibleMovesEvenRow = new Vector2Int[]
            {
                new Vector2Int(currentGridPos.x, currentGridPos.y - 1),     // Up
                new Vector2Int(currentGridPos.x, currentGridPos.y + 1),     // Down
                new Vector2Int(currentGridPos.x - 1, currentGridPos.y - 1), // Left-Up
                new Vector2Int(currentGridPos.x - 1, currentGridPos.y + 1), // Left-Down
                new Vector2Int(currentGridPos.x - 1, currentGridPos.y),     // Left
                new Vector2Int(currentGridPos.x + 1, currentGridPos.y)      // Right
            };

            Vector2Int[] possibleMovesOddRow = new Vector2Int[]
            {
                new Vector2Int(currentGridPos.x, currentGridPos.y - 1),     // Up
                new Vector2Int(currentGridPos.x, currentGridPos.y + 1),     // Down
                new Vector2Int(currentGridPos.x - 1, currentGridPos.y),     // Left
                new Vector2Int(currentGridPos.x + 1, currentGridPos.y),     // Right
                new Vector2Int(currentGridPos.x + 1, currentGridPos.y - 1), // Right-Up
                new Vector2Int(currentGridPos.x + 1, currentGridPos.y + 1)  // Right-Down
            };

            Vector2Int[] possibleMoves = (currentGridPos.y % 2 == 0) ? possibleMovesEvenRow : possibleMovesOddRow;

            // Select a random adjacent position that is within bounds, not EarthTerrain, and passes IsWithinRange
            Vector2Int targetGridPos = Vector2Int.zero;
            bool validMoveFound = false;
            int attempts = 0;
            const int maxAttempts = 10;

            while (attempts < maxAttempts)
            {
                targetGridPos = possibleMoves[Random.Range(0, possibleMoves.Length)];
                if (targetGridPos.x >= 0 && targetGridPos.x < placeTile.width && targetGridPos.y >= 0 && targetGridPos.y < placeTile.height)
                {
                    Vector3 worldPos = placeTile.Grid[targetGridPos.x, targetGridPos.y];
                    worldPos.z = -1f;
                    if (!playerMovement.IsEarthTerrain(worldPos) && playerMovement.IsWithinRange(currentGridPos, targetGridPos))
                    {
                        validMoveFound = true;
                        break;
                    }
                }
                attempts++;
            }

            Assert.IsTrue(validMoveFound, $"Could not find a valid adjacent tile to move to after {maxAttempts} attempts on move {moves + 1}!");

            Vector3 targetWorldPos = placeTile.Grid[targetGridPos.x, targetGridPos.y];
            targetWorldPos.z = -1f; // Match tank's Z

            // Directly set the target position and initiate movement
            playerMovement.SetTargetAndMove(targetWorldPos);
            Debug.Log($"Move {moves + 1}: Target grid position: {targetGridPos}, World position: {targetWorldPos}");

            // Wait for the movement to complete (based on moveSpeed = 5f and typical hex distance)
            yield return new WaitForSeconds(1.0f); // Adjust based on moveSpeed and distance

            Vector3 newPosition = playerTank.transform.position;
            Debug.Log($"Move {moves + 1}: Previous Position: {previousPosition}, New Position: {newPosition}");

            // Check if the position has changed
            Assert.AreNotEqual(previousPosition, newPosition, $"Player tank did not move on move {moves + 1}!");

            // Verify final position is on a tile using FindNearestTile
            Vector3 nearestTilePos = playerMovement.FindNearestTile(newPosition);
            bool isOnTile = Vector3.Distance(newPosition, nearestTilePos) < 0.1f;
            Debug.Log($"Move {moves + 1}: Tank check: Is on tile: {isOnTile}, Nearest tile: {nearestTilePos}");
            Assert.IsTrue(isOnTile, $"New position {newPosition} is not on a tile on move {moves + 1}!");

            // Update for the next iteration
            previousPosition = newPosition;
            currentGridPos = playerMovement.WorldToGridPosition(newPosition);
            moves++;
        }

        Assert.Pass($"Player Tank made {maxMoves} moves without going out of bounds");
    }
}