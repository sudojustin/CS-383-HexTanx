using UnityEngine;

public class AIControl : MonoBehaviour
{
    private TankType tank;
    private PlaceTile placeTileScript;
    private PlayerTank playerTank;
    [SerializeField]
    private GameObject projectilePrefab;
    private AudioClip shootSoundOverride;

    public void Start()
    {
        tank = GetComponent<TankType>();
        if (tank == null)
        {
            Debug.LogError("AIControl: No TankType found on " + gameObject.name);
            InvokeRepeating(nameof(MakeDecision), 1.0f, 2.0f);
        }
        // Get a reference to the PlaceTile script
        placeTileScript = FindObjectOfType<PlaceTile>();
        if (placeTileScript == null)
        {
            Debug.LogError("AIControl: No PlaceTile found in the scene!");
        }

        playerTank = FindObjectOfType<PlayerTank>();
        if (playerTank == null)
        {
            Debug.Log("AICONTROL: No PlayerTank found in the scene!");
        }

    }


    public void MakeDecision()
    {
        if (tank == null) return;

        tank.GenerateRandomDecisions();

        if (tank.ShouldShoot())
        {
            ShootAtPlayer();
        }
        else
        {
            MoveToNewLocation();
        }
    }

    private void ShootAtPlayer()
    {
         Debug.Log("MakeDecision was shootatplayer");
         if(playerTank == null)
         {
             Debug.Log("player tank in AIControl:ShootAtPlayer is null");
             return;
         }
         Vector3 targetPosition = playerTank.GetTankLocation();
         Vector3 direction = (targetPosition - transform.position).normalized;
         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
         Quaternion rotation = Quaternion.Euler(0, 0, angle);
         GameObject bullet = Instantiate(projectilePrefab, transform.position, rotation);
         EnemyProjectile projectileScript = bullet.GetComponent<EnemyProjectile>();
         Debug.Log("Vector3, bullet gamebobject");

         if (tank.ShotHitsPlayer())
         {
             Debug.Log(gameObject.name + " shot hit the player!");
            // Implement damage logic for player
            if (projectileScript != null)
            {
                projectileScript.SetTarget(targetPosition);
            }
        }
        else
        {
            Debug.Log(gameObject.name + " shot missed!");
            targetPosition.x = targetPosition.x - 1.0f;
            if (projectileScript != null)
            {
                projectileScript.SetTarget(targetPosition);
            }
        }
    }

    public void MoveToNewLocation()
    {
        Vector3 newLocation = GetRandomAdjacentHex();
        if (IsWithinMapBounds(newLocation)) // Check if the new location is within the grid bounds
        {
            tank.UpdateTankLocation(newLocation);
            Debug.Log(gameObject.name + " moved to " + newLocation);
        }
        else
        {
            Debug.Log(gameObject.name + " tried to move out of bounds, staying in place." + newLocation);
            MoveToNewLocation();
        }
    }

    private Vector3 GetRandomAdjacentHex()
    {
        float hexHeight = Mathf.Sqrt(3) / 2f;
        Vector3[] possibleMoves = {
            new Vector3(tank.tankLocation.x + 1.0f, tank.tankLocation.y, -1), //East
            new Vector3(tank.tankLocation.x - 1.0f, tank.tankLocation.y, -1), //West
            new Vector3(tank.tankLocation.x + 0.5f, tank.tankLocation.y + hexHeight, -1), // NorthEast
            new Vector3(tank.tankLocation.x - 0.5f, tank.tankLocation.y + hexHeight, -1), //NorthWest
            new Vector3(tank.tankLocation.x + 0.5f, tank.tankLocation.y - hexHeight, -1), //SouthEast
            new Vector3(tank.tankLocation.x - 0.5f, tank.tankLocation.y - hexHeight, -1), //SouthWest
        };

        return possibleMoves[Random.Range(0, possibleMoves.Length)];
    }
    private bool IsWithinMapBounds(Vector3 position)
    {
        float hexHeight = Mathf.Sqrt(3) / 2f;
        if (position.x > 0f && position.x <= placeTileScript.width && position.y >= 0.00f && position.y < (placeTileScript.height - hexHeight- 0.3f))
        {
            return true;
        }

        return false;
    }
}
