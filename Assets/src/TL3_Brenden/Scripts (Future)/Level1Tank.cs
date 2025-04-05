using UnityEngine;

public class Level1Tank : TankType
{
    public override void Initialize()
    {
        health = 50;
        enemyActionPoints = 2;
        // Debug.Log("Level 1 Tank Spawned with " + health + " HP");
        damage = 10;
        //damage = DamageScriptBC.getDamage();
    }
}
