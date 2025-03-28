using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    private GameObject enemyTank;

    public BattleState state;

    private PlayerTank playerTank;
    private AIControl aiControl;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = BattleState.START;
        Invoke("SetupBattle", .1f);
    }

    void SetupBattle()
    {
        Debug.Log("SetupBattle started.");

        PlayerTankSpawner playerSpawner = FindObjectOfType<PlayerTankSpawner>();
        Debug.Log(playerSpawner != null ? "PlayerTankSpawner found!" : "PlayerTankSpawner NOT found!");
        if (playerSpawner != null)
        {
            Debug.Log("PlayerTankSpawner found.");
            playerSpawner.SpawnPlayerTankWithPosition();
        }
        else
        {
            Debug.LogError("BattleSystem: PlayerTankSpawner not found in the scene!");
        }

        EnemyTankSpawner enemySpawner = FindObjectOfType<EnemyTankSpawner>();
        if (enemySpawner != null)
        {
            Debug.Log("EnemyTankSpawner found.");
            enemySpawner.SpawnEnemyTankWithRandomPosition();
        }
        else
        {
            Debug.LogError("BattleSystem: EnemyTankSpawner not found in the scene!");
        }

        Debug.Log("SetupBattle completed.");
        state = BattleState.PLAYERTURN;
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        playerTank = FindObjectOfType<PlayerTank>(); // Find player tank

        if (playerTank == null)
        {
            Debug.LogError("PlayerTank not found!");
            Invoke("GameLost", 1.0f);
        }
        playerTank.SetActionPoints(3); // Reset action points for new turn
        Debug.Log("Player Turn Started! Action Points: " + playerTank.GetActionPoints());
    }

    public void PlayerActionTaken()
    {
        if (state != BattleState.PLAYERTURN) return; // Ensure it's still the player's turn
        Debug.Log("PlayerActionTaken() called. Remaining Action Points: " + playerTank.GetActionPoints());

        if (playerTank.GetActionPoints() <= 0)
        {
            Debug.Log("Player out of action points, ending turn...");
            EndPlayerTurn();
        }
    }

    void EndPlayerTurn()
    {
        Debug.Log("Player turn ended!");
        state = BattleState.ENEMYTURN;
        Invoke("StartEnemyTurn", 2.5f); // Delay enemy turn start
    }

    void StartEnemyTurn()
    {
        Debug.Log("Enemy turn started!");
        // Handle enemy AI behavior here
        enemyTank = GameObject.FindWithTag("EnemyTank");
        if (enemyTank == null)
        {
            Debug.Log("No enemy tank found. Ending enemy turn.");
            Invoke("GameWon", 1.0f);
            return;
        }
        aiControl = enemyTank.GetComponent<AIControl>();
        TankType enemyTankType = enemyTank.GetComponent<TankType>();

        if (enemyTankType != null && enemyTankType.health <= 0)
        {
            Debug.Log("Enemy tank is destroyed. Ending enemy turn.");
            EndEnemyTurn();
            return;
        }
        aiControl.MakeDecision();
        Invoke("EndEnemyTurn", 1.0f); // Delay before ending the enemy's turn
    }
    void EndEnemyTurn()
    {
        Debug.Log("Enemy turn ended!");
        state = BattleState.PLAYERTURN; // Switch back to player's turn
        StartPlayerTurn(); // Start the player's turn
    }
    void GameWon()
    {
        state = BattleState.WON;
        Debug.Log("Game Won");
    }
    void GameLost()
    {
        state = BattleState.LOST;
        Debug.Log("Game Lost");
    }

}
