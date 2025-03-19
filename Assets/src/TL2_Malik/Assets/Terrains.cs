using UnityEngine;

public class Terrains : Tiles
{
    
    private GameObject playertank;
    private int playerHealthshown;
    private int damage = 10;
    private PlayerTank playertc;
    void Start()
    {
        
       //playertank = GameObject.Find("PlayerTank");
       //PlayerTank playertc = playertank.GetComponent<PlayerTank>();
    }
    void Update()
    {

        playertank = GameObject.Find("PlayerTank");
        playertc = playertank.GetComponent<PlayerTank>();
        takeDamage();
    }
    void takeDamage()
    {

        playerHealthshown = playertc.GetHealth();
        if(playertank.transform.position.x == this.transform.position.x && playertank.transform.position.y == this.transform.position.y)
        {
            Debug.Log(": "+playerHealthshown);
            playertc.SetHealth(playerHealthshown-damage);
            playerHealthshown = playertc.GetHealth();
            Debug.Log(": "+playerHealthshown);
            
        }
    }
}
