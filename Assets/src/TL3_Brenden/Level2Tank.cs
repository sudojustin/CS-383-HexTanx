using UnityEngine;

public class Level2Tank : TankType
{
    public override void Initialize()
    {
        enemyActionPoints = 3;
        damage = 15;
        health = 100;  // Set health specific to Level 2 Tank
       // Debug.Log("Level 2 Tank Spawned with " + health + " HP");
    }
}
