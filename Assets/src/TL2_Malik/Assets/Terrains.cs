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
    private int playerHealthshown;
    //private int damage = 10;
    private TerrainDamageBC Damage;
    private PlayerTank playertc;
    void Start()
    {
        isactive = true;
        Damage = new TerrainDamage();
        Invoke("getPlayer",8f);
    }
    void Update()
    {
        
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
        if(isactive == true && playertankOBJ.transform.position.x == this.transform.position.x && playertankOBJ.transform.position.y == this.transform.position.y)
        {
            takeDamage();
            isactive = false;
            TurnOff();//Visually show what fire tiles are deactivated.
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
}
