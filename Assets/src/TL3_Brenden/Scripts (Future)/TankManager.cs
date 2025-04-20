using UnityEngine;
using UnityEngine.SceneManagement;

public class TankManager : MonoBehaviour
{
    public GameObject level1TankPrefab;
    public GameObject level2TankPrefab;
    public GameObject level3TankPrefab;
    public GameObject level4TankPrefab;
    public GameObject level666TankPrefab;
    public GameObject levelEasterTankPrefab;
    public GameObject level5TankPrefab;
    public GameObject level6TankPrefab;
    public GameObject level7TankPrefab;
    public GameObject level8TankPrefab;
    public GameObject level9TankPrefab;
    public GameObject level10TankPrefab;

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
            case "Level666":
                return level666TankPrefab;
            case "EasterLevel":
                return levelEasterTankPrefab;
            case "Level5":
                return level5TankPrefab;
            case "Level6":
                return level6TankPrefab;
            case "Level7":
                return level7TankPrefab;
            case "Level8":
                return level8TankPrefab;
            case "Level9":
                return level9TankPrefab; ;
            case "Level10":
                return level10TankPrefab;
            default:
                //Debug.LogWarning("TankManager: Unknown scene name, defaulting to Level1 tank.");
                return level1TankPrefab;
        }
    }
}
