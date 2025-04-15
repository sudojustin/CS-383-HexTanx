using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level2Tank : TankType
{
    public override void Initialize()
    {
        enemyActionPoints = 3;
        health = 100;  // Set health specific to Level 2 Tank
        // Debug.Log("Level 2 Tank Spawned with " + health + " HP");
        damage = findDamageVal(15);
    }

    public int findDamageVal(int baseDamage)
    {
        bool bcModeOn = PlayerPrefs.GetInt("BCMode", 0) == 1;

        DamageEffect damageEffect;
        if (bcModeOn)
        {
            damageEffect = new BCEnemyDamage(baseDamage);
        }
        else
        {
            damageEffect = new DamageEffect(baseDamage);
        }
        return damageEffect.GetDamage();
    }
}