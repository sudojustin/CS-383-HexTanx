using UnityEngine;


public class TerrainDamageBC
{
    //int virtural getDamage()
   public virtual int getDamage()//damage is 0 in BC mode.
    {
        return 0;
    }
}

public class TerrainDamage: TerrainDamageBC
{
    //int override getDamage()
    public override int getDamage()//Normal damage
    {
        return 10;
    }
}

public class Terrains : Tiles
{
    private bool isactive = true;
    
    private GameObject playertankOBJ;
    private GameObject enemyTank;
    private int playerHealthshown;
    private int enemyHealthShown;
    //private int damage = 10;
    private TerrainDamageBC Damage;
    private PlayerTank playertc;
    private TankType enemytc;
    void Start()
    {
        isactive = true;
        Damage = new TerrainDamage();
        InvokeRepeating("getPlayer", 8f, 10f);  // Periodically check for player
        InvokeRepeating("getEnemy", 8f, 10f);
    }
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
        //check is player has already been on this tile and if they havent they take damage.
        if(playertankOBJ != null && playertc != null)
        {
            if(isactive == true && playertankOBJ.transform.position.x == this.transform.position.x && playertankOBJ.transform.position.y == this.transform.position.y)
            {
                takeDamage();
                isactive = false;
                TurnOff();//Visually show what fire tiles are deactivated.
            }
        }
        if(enemytc != null)
        {
            if(isactive == true && Vector3.Distance(enemyTank.transform.position, this.transform.position)<1.1f)
            {
                //Debug.LogError("Tank in fire!!!");
                enemyTakeDamage();
                isactive = false;
                TurnOff();
            }
        }
        else
        {
            Debug.LogError("!!!---No Enemy Tank Component---!!!");
        }
        
            
    }
    void takeDamage()
    {

        playerHealthshown = playertc.GetHealth();
        if(playertankOBJ.transform.position.x == this.transform.position.x && playertankOBJ.transform.position.y == this.transform.position.y)
        {
            playertc.SetHealth(playerHealthshown-Damage.getDamage());
            playerHealthshown = playertc.GetHealth(); 
        }
    }
    void enemyTakeDamage()
    {
        enemytc.health -= Damage.getDamage();
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
}
       
    
