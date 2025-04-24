using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; // Normal tank shell prefab
    [SerializeField] private GameObject missileProjectilePrefab; // New missile projectile prefab
    [SerializeField] private AudioClip shootSoundOverride;
    [SerializeField] private AudioClip missileShootSoundOverride; // Optional: Different sound for missile
    [SerializeField] private float angleOffset = 0f;
    [SerializeField] private Transform barrelTip;
    private Camera mainCamera;
    private PlayerTank playerTank;
    private PlaceTile placeTile;
    private PlayerModeManager modeManager;

    void Start()
    {
        mainCamera = Camera.main;
        playerTank = GetComponentInParent<PlayerTank>();
        if (playerTank == null)
        {
            Debug.LogError("PlayerTank component not found in parent!");
        }
        placeTile = FindObjectOfType<PlaceTile>();
        if (placeTile == null)
        {
            Debug.LogError("PlaceTile not found in scene!");
        }

        modeManager = FindObjectOfType<PlayerModeManager>();
        if (modeManager == null)
        {
            Debug.LogError("PlayerModeManager not found in scene!");
        }
        if (barrelTip == null)
        {
            barrelTip = transform.Find("BarrelTip");
            if (barrelTip == null)
            {
                Debug.LogError("BarrelTip not found! Please add a BarrelTip child to the Turret.");
            }
        }
    }

    void Update()
    {
        // Rotate the turret to face the mouse
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3 direction = mouseWorldPos - transform.position;
        direction.z = 0;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Left-click to shoot (only in Shoot Mode)
        if (modeManager.GetCurrentMode() == PlayerModeManager.PlayerMode.Shoot)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (playerTank.GetActionPoints() <= 0)
                {
                    Debug.Log("No action points remaining!");
                    return;
                }
                if (playerTank.GetAmmoCount() <= 0)
                {
                    Debug.Log("No ammo remaining!");
                    return;
                }

                Vector3 targetTilePos = FindNearestTile(mouseWorldPos);
                if (targetTilePos != Vector3.zero)
                {
                    if (playerTank.UseActionPoint())
                    {
                        Shoot(targetTilePos);
                        playerTank.SetAmmoCount(playerTank.GetAmmoCount() - 1);
                        Debug.Log("Shot fired toward tile center: " + targetTilePos);
                        FindObjectOfType<BattleSystem>().PlayerActionTaken();
                    }
                }
                else
                {
                    Debug.Log("Mouse position is not over a valid tile - shot cancelled.");
                }
            }
        }
    }

    void Shoot(Vector3 targetPosition)
    {
        // Determine if we should use the missile or normal projectile
        bool useMissile = playerTank.HasMissile();
        Debug.Log($"Has Missile: {useMissile}");

        GameObject prefabToUse = useMissile ? missileProjectilePrefab : projectilePrefab;
        Debug.Log($"Prefab to use: {(prefabToUse != null ? prefabToUse.name : "null")}");

        AudioClip soundToPlay = useMissile ? missileShootSoundOverride : shootSoundOverride;

        if (soundToPlay != null)
        {
            SoundManager.GetInstance().Play(soundToPlay);
        }
        else
        {
            SoundManager.GetInstance().ShootSound();
        }
        Debug.Log("Shoot sound triggered via SoundManager");

        if (barrelTip != null)
        {
            GameObject bullet = Instantiate(prefabToUse, barrelTip.position, transform.rotation);
            Debug.Log($"Instantiated bullet: {bullet.name}");

            if (useMissile)
            {
                MissileProjectile missileScript = bullet.GetComponent<MissileProjectile>();
                Debug.Log($"Missile script found: {missileScript != null}");
                if (missileScript != null)
                {
                    missileScript.SetTarget(targetPosition);
                    Debug.Log($"Missile damage: {missileScript.damage}");
                    playerTank.ActivateMissile(false); // Consume the missile (now matches the signature)
                    Debug.Log("Missile consumed after firing");
                }
                else
                {
                    Debug.LogWarning("MissileProjectile component not found on instantiated bullet!");
                }
            }
            else
            {
                Projectile projectileScript = bullet.GetComponent<Projectile>();
                Debug.Log($"Projectile script found: {projectileScript != null}");
                if (projectileScript != null)
                {
                    projectileScript.SetTarget(targetPosition);
                    Debug.Log($"Projectile damage: {projectileScript.damage}");
                }
                else
                {
                    Debug.LogWarning("Projectile component not found on instantiated bullet!");
                }
            }
        }
        else
        {
            Debug.LogWarning("BarrelTip is null - spawning projectile at turret position instead.");
            GameObject bullet = Instantiate(prefabToUse, transform.position, transform.rotation);
            Debug.Log($"Instantiated bullet: {bullet.name}");

            if (useMissile)
            {
                MissileProjectile missileScript = bullet.GetComponent<MissileProjectile>();
                Debug.Log($"Missile script found: {missileScript != null}");
                if (missileScript != null)
                {
                    missileScript.SetTarget(targetPosition);
                    Debug.Log($"Missile damage: {missileScript.damage}");
                    playerTank.ActivateMissile(false); // Consume the missile
                    Debug.Log("Missile consumed after firing");
                }
                else
                {
                    Debug.LogWarning("MissileProjectile component not found on instantiated bullet!");
                }
            }
            else
            {
                Projectile projectileScript = bullet.GetComponent<Projectile>();
                Debug.Log($"Projectile script found: {projectileScript != null}");
                if (projectileScript != null)
                {
                    projectileScript.SetTarget(targetPosition);
                    Debug.Log($"Projectile damage: {projectileScript.damage}");
                }
                else
                {
                    Debug.LogWarning("Projectile component not found on instantiated bullet!");
                }
            }
        }
    }

    private Vector3 FindNearestTile(Vector3 position)
    {
        if (placeTile == null || placeTile.Grid == null)
        {
            Debug.LogError("PlaceTile or Grid not available");
            return Vector3.zero;
        }

        Vector3 nearestPos = placeTile.Grid[0, 0];
        float minDistance = Vector3.Distance(position, nearestPos);

        for (int x = 0; x < placeTile.Grid.GetLength(0); x++)
        {
            for (int y = 0; y < placeTile.Grid.GetLength(1); y++)
            {
                float distance = Vector3.Distance(position, placeTile.Grid[x, y]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPos = placeTile.Grid[x, y];
                }
            }
        }

        float maxTileDistance = 0.5f;
        if (minDistance <= maxTileDistance)
        {
            return new Vector3(nearestPos.x, nearestPos.y, -1);
        }
        else
        {
            return Vector3.zero;
        }
    }

    public void ShootAtEnemy()
    {
        GameObject enemyTank = GameObject.FindWithTag("EnemyTank");
        if (enemyTank == null)
        {
            Debug.LogWarning("Enemy tank not found in the scene!");
            return;
        }

        Vector3 enemyPosition = enemyTank.transform.position;
        Shoot(enemyPosition);
        Debug.Log("Shot fired toward enemy at tile center: " + enemyPosition);
    }
}