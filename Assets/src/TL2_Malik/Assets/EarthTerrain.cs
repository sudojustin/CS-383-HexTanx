using UnityEngine;
using System.Collections.Generic;

public class EarthTerrain : Tiles
{
    private List<GameObject> Bullets = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Bullets.Count == 0)
        {
           getProjectile();
           return; 
        }
        
        for(int i = Bullets.Count-1; i >= 0; i--)
        {
            GameObject bullet = Bullets[i];
            if(bullet == null)
            {
                Bullets.RemoveAt(i);
                continue;
            }

            if(Vector3.Distance(bullet.transform.position, this.transform.position)<1.1f)
            {
                Debug.Log("Destorying Bullet!");
                Destroy(bullet);
                Bullets.RemoveAt(i);
            }

        }
        /*
        if(Bullet != null)
        {
            if(Vector3.Distance(Bullet.transform.position, this.transform.position) < 1.1f)
            {
                Debug.Log("Destorying Bullet!");
                Destroy(Bullet);
                Bullet = null;
            }
        }
        */
    }

    private void getProjectile()
    {
        GameObject[] foundBullets = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject bullet in foundBullets)
        {
            if(!Bullets.Contains(bullet))
            {
                Bullets.Add(bullet);
                //Debug.Log("Projectile found!");
            }
        }
        
    }
}
