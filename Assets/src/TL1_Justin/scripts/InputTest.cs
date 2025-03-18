using UnityEngine;

public class InputTest : MonoBehaviour
{
    void Update()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                Debug.Log("Key Pressed: " + key);
            }
        }
    }
}
