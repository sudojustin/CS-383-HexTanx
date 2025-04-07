using UnityEngine;
using UnityEngine.SceneManagement;

public class TankManager : MonoBehaviour
{
    public GameObject level1TankPrefab;
    public GameObject level2TankPrefab;
    public GameObject level3TankPrefab;
    public GameObject level4TankPrefab;

    public GameObject GetTankForCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        switch (currentSceneName)
        {
            case "Level1":
                return level1TankPrefab;
            case "Level2":
                return level2TankPrefab;
            case "Level3":
                return level3TankPrefab;
            case "Level4":
                return level4TankPrefab;
            default:
                //Debug.LogWarning("TankManager: Unknown scene name, defaulting to Level1 tank.");
                return level1TankPrefab;
        }
    }
}
