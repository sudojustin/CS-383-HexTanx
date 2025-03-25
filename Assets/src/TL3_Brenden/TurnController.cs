using UnityEngine;

public class TurnController : MonoBehaviour
{
    private PlayerTank player;
    private AIControl enemyAI;
    private bool isPlayerTurn = true;

    void Start()
    {
        player = FindObjectOfType<PlayerTank>();
        enemyAI = FindObjectOfType<AIControl>();

        if (player == null)
            Debug.LogError("TurnController: No PlayerTank found in the scene!");
        if (enemyAI == null)
            Debug.LogError("TurnController: No AIControl found in the scene!");

        StartPlayerTurn();
    }

    void Update()
    {
        if (isPlayerTurn && player.GetActionPoints() <= 0)
        {
            EndPlayerTurn();
        }
    }

    public void StartPlayerTurn()
    {
        isPlayerTurn = true;
        player.SetActionPoints(3); // Reset player action points
        Debug.Log("Player's turn started!");
    }

    public void EndPlayerTurn()
    {
        Debug.Log("Player's turn ended!");
        isPlayerTurn = false;
        Invoke(nameof(StartEnemyTurn), 1f); // Small delay before AI acts
    }

    public void StartEnemyTurn()
    {
        Debug.Log("Enemy's turn started!");
        enemyAI.MakeDecision();
        Invoke(nameof(EndEnemyTurn), 1f); // Give time for the action to complete
    }

    public void EndEnemyTurn()
    {
        Debug.Log("Enemy's turn ended!");
        StartPlayerTurn();
    }
}
