using UnityEngine;



public abstract class Ptank : MonoBehaviour
{

    public int ammoCount;     // Default ammo
    public int armorCount;
    public int health;  
    public int actionPoints;   // Default action points (e.g., 3 per turn)
    // Default armor
    private Vector3 tankLocation;   // Position of the tank

    public abstract void Initialize();
    void Start()
    {
        Initialize();
        tankLocation = transform.position;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetAmmoCount()
    {
        return ammoCount;
    }

    public int GetArmorCount()
    {
        return armorCount;
    }


    public int GetActionPoints()
    {
        return actionPoints;
    }

    // Setters
    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    public void SetAmmoCount(int newAmmo)
    {
        ammoCount = newAmmo;
        if (ammoCount < 0)
        {
            ammoCount = 0;
        }
    }

    public void SetArmorCount(int newArmor)
    {
        armorCount = newArmor;
        if (armorCount < 0)
        {
            armorCount = 0;
        }
    }


    public void SetActionPoints(int newPoints)
    {
        actionPoints = newPoints;
        if (actionPoints < 0)
        {
            actionPoints = 0;
        }
    }

    public bool UseActionPoint()
    {
        if (actionPoints > 0)
        {
            actionPoints--;
            Debug.Log($"Action point used. Remaining: {actionPoints}");
            return true;
        }
        Debug.Log("No action points remaining!");
        return false;
    }

    // Getters

    public Vector3 GetTankLocation()
    {
        return tankLocation;
    }

    public void SetTankLocation(Vector3 newLocation)
    {
        tankLocation = newLocation;
        transform.position = newLocation;
        Debug.Log("Tank location set to " + newLocation);
    }

   
}