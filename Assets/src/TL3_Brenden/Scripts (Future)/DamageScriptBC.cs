using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageEffect
{
    protected int baseDamage;

    public DamageEffect(int baseDamage)
    {
        this.baseDamage = baseDamage;
    }

    public virtual int GetDamage()
    {
        return baseDamage;
    }
}

public class BCEnemyDamage : DamageEffect
{
    public BCEnemyDamage(int baseDamage) : base(baseDamage) { }

    public override int GetDamage()
    //public int GetDamage()
    {
        //Debug.Log("BC MODE DAMAGE ENEMY ENABLED");
        return 1;       //BC mode damage
    }
}
