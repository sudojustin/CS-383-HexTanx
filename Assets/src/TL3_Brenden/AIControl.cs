using UnityEngine;

public class AIControl : MonoBehaviour
{
    private TankType tank;

    void Start()
    {
        tank = GetComponent<TankType>();
        if (tank == null)
        {
            Debug.LogError("AIControl: No TankType found on " + gameObject.name);
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
        if (tank.ShotHitsPlayer())
        {
            Debug.Log(gameObject.name + " shot hit the player!");
            // Implement damage logic for player
        }
        else
        {
            Debug.Log(gameObject.name + " shot missed!");
        }
    }

    private void MoveToNewLocation()
    {
        Vector3 newLocation = GetRandomAdjacentHex();
        tank.UpdateTankLocation(newLocation);
        Debug.Log(gameObject.name + " moved to " + newLocation);
    }

    private Vector3 GetRandomAdjacentHex()
    {
        float hexHeight = Mathf.Sqrt(3) / 2f;
        Vector3[] possibleMoves = {
            new Vector3(tank.tankLocation.x + 1, tank.tankLocation.y, 0),
            new Vector3(tank.tankLocation.x - 1, tank.tankLocation.y, 0),
            new Vector3(tank.tankLocation.x + 0.5f, tank.tankLocation.y + hexHeight, 0),
            new Vector3(tank.tankLocation.x - 0.5f, tank.tankLocation.y + hexHeight, 0),
            new Vector3(tank.tankLocation.x + 0.5f, tank.tankLocation.y - hexHeight, 0),
            new Vector3(tank.tankLocation.x - 0.5f, tank.tankLocation.y - hexHeight, 0),
        };

        return possibleMoves[Random.Range(0, possibleMoves.Length)];
    }
}
