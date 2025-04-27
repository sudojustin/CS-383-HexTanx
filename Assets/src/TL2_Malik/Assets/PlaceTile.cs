using UnityEngine;

public class PlaceTile : MonoBehaviour
{
    //for testing
    public bool runOnStart = true;
    //random number to chose between the diffrent tile
    private int choseTerrain = 0; 
    //random number to chose if its a normal tile 
    private int isTerrain = 0;
    //width of the tiles
    private float hexwidth = 1f;
    //height of the tiles
    public float hexHeight = Mathf.Sqrt(3) / 2f;
    //space between tiles
    public float heightspacing;
    //width of the grid
    public int width;
    //height of the grid
    public int height;
    //y posistion of the center of each tile
    public float ypos;
    //x position of the center of each tile
    public float xpos;
    //an offset to make the grid line up
    public float offset;
    //second offset
    public float offset2;
    //minimum the width and height of the grid can be
    public int min;
    //maximum the width and height of the grid can be
    public int max;
    //normal tile. The Superclass for all tiles
    public GameObject Tile;
    //The fire tiles
    public GameObject Terrain1;
    //the earth tiles
    public GameObject Terrain2;
    //the ice tiles
    public GameObject Terrain3;
    //vector to store x and y positions of each tile
    public Vector3[,] Grid;

    [SerializeField]
    //main camrera
    private Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get main camera
        cam = Camera.main;  
        if (cam == null)
        {
            Debug.LogError("Main camera is not assigned or not found!");
            return;
        }
        //set grid height
        height = Random.Range(min,max);
        //set grid width
        width = Random.Range(min, max);
        Grid = null;
        //make grid
        if(runOnStart)
        {
            MakeMap(width, height+2);
        }
        
    }
    //function to make the grid. 
    public void MakeMap(int width, int height)
    {  
        Grid = new Vector3[width, height];
        if (Grid == null)
        {
            Grid = new Vector3[width, height];
        }
       for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                TileFactory(x,y);
            }
        }

        if (cam != null)
        {
            cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
        }
        else
        {
            Debug.LogError("Camera (cam) is not assigned!");
        }
    }
    //function to control what tiles are placed. Factory pattern, this class knows nothing about the tiles
    void TileFactory(int x, int y)
    {
        isTerrain = Random.Range(1,3);
        choseTerrain = Random.Range(1,4);
        xpos = x * hexwidth;
        ypos = y * hexHeight * 1.0f;
            
                if(y % 2 == 1)
                {
                    xpos += offset;
                }
                if(isTerrain == 2)
                {
                    if(choseTerrain == 1)
                    {
                        Grid[x, y] = new Vector3(xpos+=hexwidth/2f, ypos, 0);
                        Instantiate(Terrain2,Grid[x, y],Quaternion.identity);
                    }
                    else if(choseTerrain == 2)
                    {
                        Grid[x, y] = new Vector3(xpos+=hexwidth/2f, ypos, 0);
                        Instantiate(Terrain3,Grid[x, y],Quaternion.identity);
                    }
                    else
                    {
                        Grid[x, y] = new Vector3(xpos+=hexwidth/2f, ypos, 0);
                        Instantiate(Terrain1,Grid[x, y],Quaternion.identity);
                    }
                }
                else
                {
                    Grid[x, y] = new Vector3(xpos+=hexwidth/2f, ypos, 0); 
                    Instantiate(Tile,Grid[x, y],Quaternion.identity);
                }
    }
    
}

