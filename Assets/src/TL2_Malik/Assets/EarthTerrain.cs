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
        getProjectile();
        if(Bullet != null)
        {
            if(Vector3.Distance(Bullet.transform.position, this.transform.position) < 1f)
            {
                Destroy(Bullet);
            }
        }
    }

    private void getProjectile()
    {
        Bullet = GameObject.Find("Projectile");
        if(Bullet != null)
        {
            Debug.Log("Projectile found!");
        }
        
    }
}
