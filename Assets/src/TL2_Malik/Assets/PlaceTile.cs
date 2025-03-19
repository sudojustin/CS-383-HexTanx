using UnityEngine;

public class PlaceTile : MonoBehaviour
{
    private int isTerrain = 0;
    private float hexwidth = 1f;
    public float hexHeight = Mathf.Sqrt(3) / 2f;
    public float heightspacing;
    public int width;
    public int height;
    public float ypos;
    public float xpos;
    public float offset;
    public float offset2;
    public int min;
    public int max;
    public GameObject Tile;
    public GameObject Terrain;
    public Vector3[,] Grid;

    private GameObject playertank;
   

    [SerializeField]
    private Transform cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playertank = GameObject.Find("PlayerTank");
        height = Random.Range(min,max);
        width = Random.Range(min,max);
        Grid = new Vector3[width,height];
        MakeMap(width, height);
        //playertank.transform.position = Grid[width/2,1];
    }

    void MakeMap(int width, int height)
    {
       for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                isTerrain = Random.Range(1,3);
                xpos = x * hexwidth;
                ypos = y * hexHeight * 1.0f;
            
                if(y % 2 == 1)
                {
                    xpos += offset;
                }
                if(isTerrain == 2)
                {
                    Grid[x, y] = new Vector3(xpos+=hexwidth/2f, ypos, 0);
                    Instantiate(Terrain,Grid[x, y],Quaternion.identity);
                }
                else
                {
                    Grid[x, y] = new Vector3(xpos+=hexwidth/2f, ypos, 0); 
                    Instantiate(Tile,Grid[x, y],Quaternion.identity);
                }
                
                
            }
        }

        cam.transform.position = new Vector3((float)width/2 -0.5f, (float)height/2 -0.5f,-10); 
    }
    
}
