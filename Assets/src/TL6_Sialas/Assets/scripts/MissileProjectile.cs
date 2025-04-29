using UnityEngine;

public class MissileProjectile : Projectile
{
    // Constructor or initialization to set damage
    public MissileProjectile()
    {
        damage = 100; // Override the damage to 50
    }

    void Start()
    {
        damage = 100; // Ensure damage is set to 50
    }

    // Inherit the rest of the behavior from Projectile
}