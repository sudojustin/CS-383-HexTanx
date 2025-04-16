using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIControl : MonoBehaviour
{
    private TankType tank;
    private PlaceTile placeTileScript;
    private PlayerTank playerTank;
    private bool isMoving = false;

    [SerializeField]
    private GameObject projectilePrefab;
    private AudioClip shootSoundOverride;
    private float moveSpeed = 1.0f;

    public void Start()
    {
        tank = GetComponent<TankType>();
        if (tank == null)
        {
            //Debug.LogError("AIControl: No TankType found on " + gameObject.name);
            //InvokeRepeating(nameof(MakeDecision), 1.0f, 2.0f); old way
        }
        // Get a reference to the PlaceTile script
        placeTileScript = FindObjectOfType<PlaceTile>();
        if (placeTileScript == null)
        {
            //Debug.LogError("AIControl: No PlaceTile found in the scene!");
        }

        playerTank = FindObjectOfType<PlayerTank>();
        if (playerTank == null)
        {
            //Debug.Log("AICONTROL: No PlayerTank found in the scene!");
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

    public void ShootAtPlayer()
    {
         //Debug.Log("MakeDecision was shootatplayer");
         if(playerTank == null)
         {
             //Debug.Log("player tank in AIControl:ShootAtPlayer is null");
             return;
         }
         Vector3 targetPosition = playerTank.GetTankLocation();
        //Debug.Log("Vector3, bullet gamebobject");
        if (tank is Level4Tank)
        {
            SoundManager.GetInstance().enemyBossShootSound();
        }
        else
        {
            SoundManager.GetInstance().ShootSound();
        }
        if (tank.ShotHitsPlayer())
         {
            Vector3 direction = (targetPosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(projectilePrefab, transform.position, rotation);
            EnemyProjectile projectileScript = bullet.GetComponent<EnemyProjectile>();
            //Debug.Log(gameObject.name + " shot hit the player!");
            // Implement damage logic for player
            if (projectileScript != null)
            {
                int damage = tank.GetDamage();
                projectileScript.SetDamage(damage);
                projectileScript.SetTarget(targetPosition);
            }
        }
        else
        {
            targetPosition.x = targetPosition.x - 1.5f;
            targetPosition.y = targetPosition.y + 2.0f;
            Vector3 direction = (targetPosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(projectilePrefab, transform.position, rotation);
            EnemyProjectile projectileScript = bullet.GetComponent<EnemyProjectile>();
            projectileScript.damage = 0;
            //Debug.Log(gameObject.name + " shot missed!");
            if (projectileScript != null)
            {
                projectileScript.SetTarget(targetPosition);
            }
        }
    }
    public void MoveToNewLocation()
    {
        Vector3 newLocation = GetRandomAdjacentHex();
        if (IsWithinMapBounds(newLocation))
        {
            if (tank is Level4Tank)
            {
                SoundManager.GetInstance().bossEnemyMoveSound();
            }
            else
            {
                SoundManager.GetInstance().EnemyMoveSound();
            }
            StartCoroutine(MoveSmoothly(newLocation));
            //Debug.Log(gameObject.name + " moving to " + newLocation);
        }
        else
        {
            //Debug.Log(gameObject.name + " attempted to move out of bounds, retrying.");
            MoveToNewLocation();
        }
    }

    private IEnumerator MoveSmoothly(Vector3 destination)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        float duration = Vector3.Distance(startPosition, destination) / moveSpeed; // Adjust speed

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, destination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = destination; // Snap to final position
        tank.UpdateTankLocation(destination);
        isMoving = false;
        SoundManager.GetInstance().StopMovementSound();
        //Debug.Log(gameObject.name + " reached destination: " + destination);
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
        List<Vector3> validMoves = new List<Vector3>();

        foreach (Vector3 move in possibleMoves)
        {
            if (IsWithinMapBounds(move) && !IsEarthTerrain(move))
            {
                validMoves.Add(move);
            }
        }

        if (validMoves.Count > 0)
        {
            return validMoves[Random.Range(0, validMoves.Count)];
        }

        return possibleMoves[Random.Range(0, possibleMoves.Length)];
    }
    private bool IsEarthTerrain(Vector3 position)
    {
        Collider2D tileCollider = Physics2D.OverlapPoint(position);
        if (tileCollider != null)
        {
            EarthTerrain terrain = tileCollider.GetComponent<EarthTerrain>();
            if (terrain != null)
            {
                //Debug.Log(gameObject.name + " avoiding Earth Terrain at: " + position);
                return true;
            }
        }
        return false;
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
