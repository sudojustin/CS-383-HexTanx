using UnityEngine;


/*
 * 
 * TankType and the subsequent Level1Tank, Level2Tank, Level3Tank, and Level4Tank use the Strategy pattern.
 * 
 * The Strategy pattern lets you indirectly alter the object’s behavior at runtime by associating it with
 * different sub-objects which can perform specific sub-tasks in different ways.
 * 
 * 
 */

public abstract class TankType : MonoBehaviour
{
    public int health;              //Store tanks health
    public Vector3 tankLocation;    //Store tanksLocation
    public int damage;
    public int enemyActionPoints;

    private int decisionToMoveOrShoot;  // 0 = Move, 1 = Shoot
    private int moveDirectionIndex;     // Randomized movement direction
    private int shotAccuracy;           // 0 = Miss, 1 = Hit

    public abstract void Initialize();
    

    public void GenerateRandomDecisions()
    {
        decisionToMoveOrShoot = Random.Range(0, 2);
        moveDirectionIndex = Random.Range(0, 6);  // Assuming 6 hex directions
        shotAccuracy = Random.Range(0, 2);
    }

    public bool ShouldShoot() => decisionToMoveOrShoot == 1;
    public bool ShotHitsPlayer() => shotAccuracy == 1;

    public void UpdateTankLocation(Vector3 newLocation)
    {
        tankLocation = newLocation;
        transform.position = newLocation;
    }
    public void ResetActionPoints()
    {
        enemyActionPoints = GetInitialActionPoints();
        //Debug.Log("Enemy action points reset to: " + enemyActionPoints);
    }

    private int GetInitialActionPoints()
    {
        if (this is Level1Tank) return 2;
        if (this is Level2Tank) return 3;
        if (this is Level3Tank) return 4;
        if (this is Level4Tank) return 5;
        if (this is Level4377Tank) return 6;
        if (this is LevelEasterTank) return 2;
        if (this is Level5Tank) return 5;
        if (this is Level6Tank) return 2;
        if (this is Level7Tank) return 4;
        if (this is Level8Tank) return 2;
        if (this is Level9Tank1) return 3;
        if (this is Level10Tank1) return 3;
        return 1; // Default fallback
    }
    public int GetDamage()
    {
        return damage;
    }
}
