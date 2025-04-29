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
        allEnemies = new List<AIControl>(FindObjectsOfType<AIControl>());
        allEnemies.RemoveAll(e => e == null || e.GetComponent<TankType>().health <= 0);

        if (allEnemies.Count == 0)
        {
            Debug.Log("All enemies destroyed. Player wins!");
            Invoke("DecideScene", 3.0f);
            return;
        }
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
                    if (playerTank == null || playerTank.GetHealth() <= 0)
                    {
                        GameLost();
                        yield break;
                    }
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
        string currentLevel = SceneManager.GetActiveScene().name;

        /*if(currentLevel == "Level666")
        {
            string returnLevel = PlayerPrefs.GetString("LostFromLevel", "Level1");
            SceneManager.LoadScene(returnLevel);
        }*/
        SceneManager.LoadScene("Assets/Scenes/GameOverWin.unity");
    }

    void GameLost()
    {
        state = BattleState.LOST;

        string currentLevel = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LostFromLevel", currentLevel);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Assets/Scenes/GameOverLose.unity");
    }

    void DecideScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Level666")
        {
            string returnLevel = PlayerPrefs.GetString("LostFromLevel", "Level1");
            SceneManager.LoadScene(returnLevel);
        }
        switch (currentSceneName)
        {
            case "Level1": SceneManager.LoadScene("Assets/Scenes/Level2.unity"); break;
            case "Level2": SceneManager.LoadScene("Assets/Scenes/Level3.unity"); break;
            case "Level3": SceneManager.LoadScene("Assets/Scenes/Level4.unity"); break;
            case "Level4": SceneManager.LoadScene("Assets/Scenes/Level5.unity"); break;
            case "LevelEaster": SceneManager.LoadScene("Assets/Scenes/Level5.unity"); break;
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
