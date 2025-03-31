using UnityEngine;

public class TankManager : MonoBehaviour
{
    public GameObject level1TankPrefab;
    public GameObject level2TankPrefab;
    public GameObject level3TankPrefab;
    public GameObject level4TankPrefab;

    public GameObject GetRandomTank()
    {
        int choice = Random.Range(0, 7);
        if (choice >= 0 && choice < 2) return level1TankPrefab;
        if (choice >= 2 && choice < 4) return level2TankPrefab;
        if (choice >= 4 && choice < 6) return level3TankPrefab;
        return level4TankPrefab;
    }
}
