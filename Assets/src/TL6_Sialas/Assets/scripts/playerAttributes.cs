using UnityEngine;

public class PlayerTank : Ptank
{
    public override void Initialize()
    {
        PlayerHealth heal = PlayerHealthFactory.setInitialHealth();
        health = heal.getInitialHealth();
        ammoCount = 30;     // Default ammo
        armorCount = 0;
        actionPoints = 3;   // Default action points (e.g., 3 per turn)
    }

    
}


public class PlayerHealth
{
    //int virtural getDamage()
    public virtual int getInitialHealth()
    {
        return 100;
    }
}

public class PlayerHealthBC : PlayerHealth
{

    public override int getInitialHealth()//Normal damage
    {
        return 200;
    }
}

public static class PlayerHealthFactory
{
    public static PlayerHealth setInitialHealth()
    {
        // Check if BC mode is enabled
        bool bcModeEnabled = PlayerPrefs.GetInt("BCMode", 0) == 1;

        // Return the appropriate effect based on BC mode
        if (bcModeEnabled)
        {
            return new PlayerHealthBC();
            Debug.LogError("BC player health");
        }
        else
        {
            return new PlayerHealth();
            Debug.LogError("Normal player health");
        }
    }
}



