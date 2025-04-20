/*using UnityEngine;
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
    private int actionPointHolder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = BattleState.START;
        Invoke("SetupBattle", .1f);
    }

    void SetupBattle()
    {
        //Debug.Log("SetupBattle started.");

        PlayerTankSpawner playerSpawner = FindObjectOfType<PlayerTankSpawner>();
        //Debug.Log(playerSpawner != null ? "PlayerTankSpawner found!" : "PlayerTankSpawner NOT found!");
        if (playerSpawner != null)
        {
            //Debug.Log("PlayerTankSpawner found.");
            playerSpawner.SpawnPlayerTankWithRandomPosition();
        }
        else
        {
            Debug.LogError("BattleSystem: PlayerTankSpawner not found in the scene!");
        }

        EnemyTankSpawner enemySpawner = FindObjectOfType<EnemyTankSpawner>();
        if (enemySpawner != null)
        {
            //Debug.Log("EnemyTankSpawner found.");
            enemySpawner.SpawnEnemyTankWithRandomPosition();
        }
        else
        {
            Debug.LogError("BattleSystem: EnemyTankSpawner not found in the scene!");
        }

        //Debug.Log("SetupBattle completed.");
        state = BattleState.PLAYERTURN;
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        playerTank = FindObjectOfType<PlayerTank>(); // Find player tank
        //enemyTank = GameObject.FindWithTag("EnemyTank");

        if (playerTank == null)
        {
            //Debug.LogError("PlayerTank not found!");
            Invoke("GameLost", 1.0f);
        }
        playerTank.SetActionPoints(3); // Reset action points for new turn
        //Debug.Log("Player Turn Started! Action Points: " + playerTank.GetActionPoints());
    }

    public void PlayerActionTaken()
    {       
        if(playerTank.GetHealth() <= 0)
        {
            Invoke("GameLost", 1.0f);
        }
        if (state != BattleState.PLAYERTURN) return; // Ensure it's still the player's turn
        //Debug.Log("PlayerActionTaken() called. Remaining Action Points: " + playerTank.GetActionPoints());

        if (playerTank.GetActionPoints() <= 0)
        {
            //Debug.Log("Player out of action points, ending turn...");
            EndPlayerTurn();
        }

    }

    public void EndPlayerTurn()
    {
        //Debug.Log("Player turn ended!");
        state = BattleState.ENEMYTURN;
        Invoke("StartEnemyTurn", 2.5f); // Delay enemy turn start
    }

    void StartEnemyTurn()
    {
        //Debug.Log("Enemy turn started!");
        // Handle enemy AI behavior here
        enemyTank = GameObject.FindWithTag("EnemyTank");
        if (enemyTank == null)
        {
            //Debug.Log("No enemy tank found. Ending enemy turn.");
            Invoke("DecideScene", 1.0f);
            return;
        }
        aiControl = enemyTank.GetComponent<AIControl>();
        TankType enemyTankType = enemyTank.GetComponent<TankType>();

        if (enemyTankType != null && enemyTankType.health <= 0)
        {
            //Debug.Log("Enemy tank is destroyed. Ending enemy turn.");
            EndEnemyTurn();
            return;
        }
        enemyTankType.ResetActionPoints();
        StartCoroutine(EnemyTurnRoutine(enemyTankType));
    }

    IEnumerator EnemyTurnRoutine(TankType enemyTankType)
    {
        //Debug.Log($"Enemy tank starts with {enemyTankType.enemyActionPoints} action points.");
        playerTank = FindObjectOfType<PlayerTank>();
        while (enemyTankType.enemyActionPoints > 0)
        {
            aiControl.MakeDecision(); // AI makes one decision per iteration
            enemyTankType.enemyActionPoints--; // Deduct action point
            //Debug.Log($"Enemy tank action taken. Remaining action points: {enemyTankType.enemyActionPoints}");

            yield return new WaitForSeconds(1.5f); // Delay between actions for pacing
        }

        //Debug.Log("Enemy tank out of action points. Ending turn.");
        EndEnemyTurn();
    }
    void EndEnemyTurn()
    {
        //Debug.Log("Enemy turn ended!");
        state = BattleState.PLAYERTURN; // Switch back to player's turn
        StartPlayerTurn(); // Start the player's turn
    }
    void GameWon()
    {
        state = BattleState.WON;
        //Debug.Log("Game Won");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/GameOverWin.unity");
    }
    void GameLost()
    {
        state = BattleState.LOST;
        //Debug.Log("Game Lost");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/GameOverLose.unity");
    }
    void DecideScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        //Debug.Log("DecideScene");
        if (currentSceneName == "Level1")
        {
            //Debug.Log("Loading Level2");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/Level2.unity");
        }
        else if (currentSceneName == "Level2")
        {
            //Debug.Log("Loading Level3");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/Level3.unity");
        }
        else if (currentSceneName == "Level3")
        {
            //Debug.Log("Loading Level4");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/Level4.unity");
        }
        else if (currentSceneName == "Level4")
        {
            //Debug.Log("Loading Level5");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/Level5.unity");
        }
        else if (currentSceneName == "Level5")
        {
            //Debug.Log("Loading Level6");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/Level6.unity");
        }
        else if (currentSceneName == "Level6")
        {
            //Debug.Log("Loading Level7");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/Level7.unity");
        }
        else if (currentSceneName == "Level7")
        {
            //Debug.Log("Loading Level8");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/Level8.unity");
        }
        else if (currentSceneName == "Level8")
        {
            //Debug.Log("Loading Level9");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/Level9.unity");
        }
        else if (currentSceneName == "Level9")
        {
            //Debug.Log("Loading Level10");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/Level10.unity");
        }
        else if (currentSceneName == "Level10")
        {
            //Debug.Log("Loading GameWin");
            state = BattleState.WON;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Assets/Scenes/GameOverWin.unity");
        }
    }
}*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    private PlayerTank playerTank;
    private List<AIControl> allEnemies = new List<AIControl>();

    void Start()
    {
        state = BattleState.START;
        Invoke("SetupBattle", .1f);
    }

    void SetupBattle()
    {
        PlayerTankSpawner playerSpawner = FindObjectOfType<PlayerTankSpawner>();
        if (playerSpawner != null)
            playerSpawner.SpawnPlayerTankWithRandomPosition();
        else
            Debug.LogError("BattleSystem: PlayerTankSpawner not found in the scene!");

        EnemyTankSpawner enemySpawner = FindObjectOfType<EnemyTankSpawner>();
        if (enemySpawner != null)
            enemySpawner.SpawnAllEnemies();
        else
            Debug.LogError("BattleSystem: EnemyTankSpawner not found in the scene!");

        state = BattleState.PLAYERTURN;
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        playerTank = FindObjectOfType<PlayerTank>();
        if (playerTank == null)
        {
            Invoke("GameLost", 1.0f);
            return;
        }
        playerTank.SetActionPoints(3);
    }

    public void PlayerActionTaken()
    {
        if (playerTank.GetHealth() <= 0)
        {
            Invoke("GameLost", 1.0f);
            return;
        }

        if (state != BattleState.PLAYERTURN) return;

        if (playerTank.GetActionPoints() <= 0)
        {
            EndPlayerTurn();
        }
    }

    public void EndPlayerTurn()
    {
        state = BattleState.ENEMYTURN;
        Invoke("StartEnemyTurn", 2.5f);
    }

    void StartEnemyTurn()
    {
        allEnemies = new List<AIControl>(FindObjectsOfType<AIControl>());
        allEnemies.RemoveAll(e => e == null || e.GetComponent<TankType>().health <= 0);

        if (allEnemies.Count == 0)
        {
            Invoke("DecideScene", 1.0f);
            return;
        }

        StartCoroutine(EnemyTurnRoutine());
    }

    IEnumerator EnemyTurnRoutine()
    {
        foreach (AIControl enemy in allEnemies)
        {
            if (enemy == null) continue;

            TankType enemyTank = enemy.GetComponent<TankType>();
            if (enemyTank != null)
            {
                enemyTank.ResetActionPoints();
                while (enemyTank.enemyActionPoints > 0)
                {
                    enemy.MakeDecision();
                    enemyTank.enemyActionPoints--;
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }

        EndEnemyTurn();
    }

    void EndEnemyTurn()
    {
        state = BattleState.PLAYERTURN;
        StartPlayerTurn();
    }

    void GameWon()
    {
        state = BattleState.WON;
        SceneManager.LoadScene("Assets/Scenes/GameOverWin.unity");
    }

    void GameLost()
    {
        state = BattleState.LOST;
        SceneManager.LoadScene("Assets/Scenes/GameOverLose.unity");
    }

    void DecideScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        switch (currentSceneName)
        {
            case "Level1": SceneManager.LoadScene("Assets/Scenes/Level2.unity"); break;
            case "Level2": SceneManager.LoadScene("Assets/Scenes/Level3.unity"); break;
            case "Level3": SceneManager.LoadScene("Assets/Scenes/Level4.unity"); break;
            case "Level4": SceneManager.LoadScene("Assets/Scenes/Level5.unity"); break;
            case "Level5": SceneManager.LoadScene("Assets/Scenes/Level6.unity"); break;
            case "Level6": SceneManager.LoadScene("Assets/Scenes/Level7.unity"); break;
            case "Level7": SceneManager.LoadScene("Assets/Scenes/Level8.unity"); break;
            case "Level8": SceneManager.LoadScene("Assets/Scenes/Level9.unity"); break;
            case "Level9": SceneManager.LoadScene("Assets/Scenes/Level10.unity"); break;
            case "Level10":
                state = BattleState.WON;
                SceneManager.LoadScene("Assets/Scenes/GameOverWin.unity");
                break;
        }
    }
}

