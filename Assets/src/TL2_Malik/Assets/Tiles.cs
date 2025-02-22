using UnityEngine;

public class Tiles : MonoBehaviour
{
    
    [SerializeField]
    private Color Base, Offset, hightlight;
    [SerializeField]
    private SpriteRenderer rend;

    private Color iscolor;
    public Color playerhere;
    private bool isplayer = false;

   
    private GameObject playertank;
    void Start()
    { 
        playertank = GameObject.Find("PlayerTank"); 
    }
    void Update()
    {
        if(playertank.transform.position.x == this.transform.position.x && playertank.transform.position.y == this.transform.position.y )
        {
            this.isplayer = true;
        }
        else
        {
            isplayer = false;
        }
    }
    void OnMouseEnter()
    {
        rend.color = hightlight;
    }
    void OnMouseExit()
    {
        if(isplayer)
        {
            rend.color = playerhere;
        }
        else
        {
            rend.color = Base;
        }
        
    }

    void OnMouseDown()
    {
        playertank.transform.position = new Vector3(this.transform.position.x, this.transform.position.y,-1);
        this.isplayer = true;
        //Debug.Log("Tile Clicked");
        HeightlightMove();
    }
    void OnMouseUp()
    {
        if(isplayer == false)
        {
            rend.color = Base;
        }
    }
    void HeightlightMove()
    {
        if(this.isplayer == true)
        {
            this.rend.color = playerhere;
        }
    }
}
