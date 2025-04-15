using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelEasterTank : TankType
{
    public override void Initialize()
    {
        enemyActionPoints = 3;
        health = 366;
        Debug.Log("Level 4377 Tank Spawned with " + health + " HP");
        damage = findDamageVal(20);
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