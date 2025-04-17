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
    //check if tile has already been stepped on
    private bool isactive = true;
    //refrence to the player
    private GameObject playertankOBJ;
    //refrence to enemy tank
    private GameObject enemyTank;
    //players current health
    private int playerHealthshown;
    //enemies current health
    private int enemyHealthShown;
    //Dynamiclly binded damage value
    private TerrainDamageBC Damage;
    //refrence to player tank componet 
    private PlayerTank playertc;
    //refrence to enemy tank componet
    private TankType enemytc;
    void Start()
    {
        isactive = true;
        //chose virtural or override based on BC toggle on main menu
        if (PlayerPrefs.GetInt("BCMode", 0) == 1)
        {
            Damage = new TerrainDamageBC();
        }
        else
        {
            Damage = new TerrainDamage();
        }
        // Periodically check for player and enemy
        InvokeRepeating("getPlayer", 8f, 10f);  
        InvokeRepeating("getEnemy", 8f, 10f);
    }
    void Update()
    {
        //null gaurds
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
                enemyTakeDamage();
                isactive = false;
                TurnOff();
            }
        }    
    }
    //function to damage the player
    void takeDamage()
    {

        playerHealthshown = playertc.GetHealth();
        if (playertankOBJ.transform.position.x == this.transform.position.x && playertankOBJ.transform.position.y == this.transform.position.y)
        {
            playertc.SetHealth(playerHealthshown - Damage.getDamage());
            playerHealthshown = playertc.GetHealth();
            if (playertc.GetHealth() <= 0)
            {


                SoundManager.GetInstance().ExplodeSound();
                //Destroy(playertc.gameObject);
                //BattleSystem.EndPlayerTurn();

            }
        }
    }
    //function to damage the enemy
    void enemyTakeDamage()
    {
        enemytc.health -= Damage.getDamage();
    }
    //funtion to get player object and componet refrences
    private void getPlayer()
    {
        playertankOBJ = GameObject.Find("PlayerTank");

        if (playertankOBJ != null)
        {
            playertc = playertankOBJ.GetComponent<PlayerTank>();

            if (playertc == null)
            {
                Debug.Log("PlayerTank component not found on PlayerTank object.");
            }
        }
        else
        {
            Debug.Log("PlayerTank object not found.");
        }
    }
    //funtion to get enemy object and componet refrences
    private void getEnemy()
    {
        enemyTank = GameObject.FindWithTag("EnemyTank"); 
    
        if (enemyTank != null)
        {
            // Try to get the TankType component (base class)
            enemytc = enemyTank.GetComponent<TankType>();
           
        }
       
    }
    
    
}
       
    
