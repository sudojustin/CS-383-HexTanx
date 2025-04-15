using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level1Tank : TankType
{
    public override void Initialize()
    {
        health = 50;
        enemyActionPoints = 2;
        // Debug.Log("Level 1 Tank Spawned with " + health + " HP");
        damage = findDamageVal(10);
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