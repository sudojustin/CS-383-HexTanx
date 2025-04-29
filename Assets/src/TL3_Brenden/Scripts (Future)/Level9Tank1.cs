using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level9Tank1 : TankType
{
    public override void Initialize()
    {
        enemyActionPoints = 2;
        health = 225;  // Set health specific to Level 4 Tank
        // Debug.Log("Level 9 Tank Spawned with " + health + " HP");
        damage = findDamageVal(50);
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