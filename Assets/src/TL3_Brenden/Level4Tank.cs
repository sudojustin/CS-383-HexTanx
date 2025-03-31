using UnityEngine;

public class Level4Tank : TankType
{
    public override void Initialize()
    {
        enemyActionPoints = 5;
        damage = 30;
        health = 300;  // Set health specific to Level 4 Tank
       // Debug.Log("Level 3 Tank Spawned with " + health + " HP");
    }
}