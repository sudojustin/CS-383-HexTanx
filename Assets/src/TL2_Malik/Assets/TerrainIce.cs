using UnityEngine;

public class TerrainIce : Tiles
{
      private bool isactive = true;
    
    private GameObject playertankOBJ;
    private GameObject enemyTank;
    private int playerActionPointsCurrent;
    private int enemyHealthShown;
    //private int damage = 10;
    private TerrainDamageBC Damage;
    private PlayerTank playertc;
    private TankType enemytc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isactive = true;
        Damage = new TerrainDamage();
        InvokeRepeating("getPlayer", 8f, 10f);  // Periodically check for player
        InvokeRepeating("getEnemy", 8f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemytc == null)
        {
            getEnemy();
        }
        if(playertankOBJ == null)
        {
            playertankOBJ = GameObject.Find("PlayerTank");
            if(playertc == null)
            {
                if(playertankOBJ != null)
                {
                    playertc = playertankOBJ.GetComponent<PlayerTank>(); 
                }
                
            }
        }

        if(playertankOBJ != null && playertc != null)
        {
            /*
            if(playertankOBJ.transform.position.x != this.transform.position.x && playertankOBJ.transform.position.y != this.transform.position.y)
            {
                isactive = true;
            }
            */
            if(isactive == true && playertankOBJ.transform.position.x == this.transform.position.x && playertankOBJ.transform.position.y == this.transform.position.y)
            {
                SlowMovement();
                isactive = false;
            }
            if(playertankOBJ.transform.position.x == this.transform.position.x && playertankOBJ.transform.position.y == this.transform.position.y)
            {
                isactive = false;
            }
            else
            {
                isactive = true;
            }
        }
        
        
    }
    private void getPlayer()
    {
        playertankOBJ = GameObject.Find("PlayerTank");

        if (playertankOBJ != null)
        {
            playertc = playertankOBJ.GetComponent<PlayerTank>();

            if (playertc == null)
            {
                Debug.LogError("PlayerTank component not found on PlayerTank object.");
            }
        }
        else
        {
            Debug.LogError("PlayerTank object not found.");
        }
    }
    private void getEnemy()
    {
        enemyTank = GameObject.FindWithTag("EnemyTank"); // Assuming enemy tank has the "EnemyTank" tag
    
        if (enemyTank != null)
        {
            // Try to get the TankType component (base class)
            enemytc = enemyTank.GetComponent<TankType>();
            /*
            if (enemytc != null)
            {
                // Successfully found the enemy tank, now you can access its properties
                Debug.Log("Enemy tank found: " + enemytc.GetType().Name);
            }
            else
            {
                Debug.LogError("TankType component not found on the enemy tank.");
            }
            */
        }
        /*
        else
        {
            Debug.LogError("Enemy tank not found in the scene.");
        }
        */
    }
    void SlowMovement()
    {

        playerActionPointsCurrent = playertc.GetActionPoints();
        if(playertankOBJ.transform.position.x == this.transform.position.x && playertankOBJ.transform.position.y == this.transform.position.y)
        {
            playertc.SetActionPoints(playerActionPointsCurrent-1);
        }
    }
}
