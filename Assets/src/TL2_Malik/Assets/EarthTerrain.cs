using UnityEngine;

public class EarthTerrain : Tiles
{
    private GameObject Bullet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Bullet == null)
        {
           getProjectile();
           return; 
        }
        
        if(Bullet != null)
        {
            if(Vector3.Distance(Bullet.transform.position, this.transform.position) < 1.1f)
            {
                Debug.Log("Destorying Bullet!");
                Destroy(Bullet);
                Bullet = null;
            }
        }
    }

    private void getProjectile()
    {
        Bullet = GameObject.FindGameObjectWithTag("Projectile");
        if(Bullet != null)
        {
            Debug.Log("Projectile found!");
        }
        
    }
}
