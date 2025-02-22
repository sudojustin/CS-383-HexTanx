using UnityEngine;

public class TankManager : MonoBehaviour
{
    public GameObject level1TankPrefab;
    public GameObject level2TankPrefab;
    public GameObject level3TankPrefab;

    public GameObject GetRandomTank()
    {
        int choice = Random.Range(0, 3);
        if (choice == 0) return level1TankPrefab;
        if (choice == 1) return level2TankPrefab;
        return level3TankPrefab;
    }
}
