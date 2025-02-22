using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public float mainSpeed = 100.0f;
    private float totalRun= 1.0f;
    public Transform cam;
   
    // Update is called once per frame
    void Update()
    {
        //float f = 0.0f;
        Vector3 p = GetBaseInput();

        if(p.sqrMagnitude > 0)// only move while a direction key is pressed
        { 
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        Vector3 newPosition = cam.transform.position;
          
        cam.transform.Translate(p);
          
        
    }


    private Vector3 GetBaseInput()//returns the basic values, if it's 0 than it's not active.
    { 
            Vector3 p_Velocity = new Vector3();
            if (Input.GetKey (KeyCode.W))
            {
                p_Velocity += new Vector3(0, 1 , 0);
            }
            if (Input.GetKey (KeyCode.S))
            {
                p_Velocity += new Vector3(0, -1, 0);
            }
            if (Input.GetKey (KeyCode.A))
            {
                p_Velocity += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey (KeyCode.D))
            {
                p_Velocity += new Vector3(1, 0, 0);
            }
            return p_Velocity;
    }
}

