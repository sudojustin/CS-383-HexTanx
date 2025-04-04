using UnityEngine;

public class Level3Tank : TankType
{
    public override void Initialize()
    {
        enemyActionPoints = 4;
        damage = 30;
        health = 150;  // Set health specific to Level 3 Tank
       // Debug.Log("Level 3 Tank Spawned with " + health + " HP");
    }
}