using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ----------------- Summary -----------------
//  This class is responsible for holding the core of the gameplay,
//  such as game duration, enemy spawn and score. It also calls the
//  functions on UIManager to update the HUD based on the data here stored.
// -------------------------------------------

public class GameplayManager : MonoBehaviour
{
    private UIManager _uiManager;
    private int _scoreAmount;

    [HideInInspector] public int GameDurationInSecounds;
    [HideInInspector] public float MatchTimer;
    [HideInInspector] public int EnemySpawnInterval;

    private void Awake()
    {
        // Changes the Seed of Random function to improve unpredictability.
        Random.InitState(System.DateTime.Now.Second);
        Time.timeScale = 1;

        GameDurationInSecounds = 180;
        EnemySpawnInterval = 15;

        DontDestroyOnLoad(this.gameObject);
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name != "GameScene")
        {
            return;
        }
        
        MatchTimer -= Time.deltaTime;

        if (MatchTimer <= 0)
        {
            EndGame();
        }
    }

    // Keeps this manager active so the objects in game can use its data.
    private void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
            MatchTimer = GameDurationInSecounds;
        }
    }

    // Changes the total score and updates the UI to match the current score.
    public void UpdateScore(int amount)
    {
        _scoreAmount += amount;

        if (_scoreAmount < 0)
        {
            _scoreAmount = 0;
        }

        _uiManager.UpdateUIScore(_scoreAmount);
    }

    // Calls the UI to update the HUD.
    public void UpdatePlayerCurrentAmmo(int ammoAmount)
    {
        _uiManager.UpdateAmmoIcons(ammoAmount);
    }

    // Pauses the game and updates the UI.
    public void EndGame()
    {
        _uiManager.ShowEndGameMenu(_scoreAmount);
        Time.timeScale = 0;
    }
}
