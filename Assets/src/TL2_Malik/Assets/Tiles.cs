using UnityEngine;

public class Tiles : MonoBehaviour
{

    [SerializeField]
    public Color Base, Offset, hightlight;
    [SerializeField]
    public SpriteRenderer rend;

    private Color iscolor;
    public Color playerhere;
    


    //public GameObject playertank;
    void Start()
    {
    
    }
    

    void OnMouseEnter()
    {
        rend.color = hightlight;
    }
    void OnMouseExit()
    {
        rend.color = Base;
    }
    public void TurnOff()
    {
        rend.color = Offset;
        Base = Offset;
    }

    void OnMouseUp()
    {
        rend.color = Base;
    }
}   
