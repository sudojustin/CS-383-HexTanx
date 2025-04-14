using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level4377Tank : TankType
{
    public override void Initialize()
    {
        enemyActionPoints = 1;
        health = 666;
        Debug.Log("Level 4377 Tank Spawned with " + health + " HP");
        damage = findDamageVal(6);
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