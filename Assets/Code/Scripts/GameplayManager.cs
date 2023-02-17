using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    private UIManager _uiManager; 
    private int _scoreAmount;

    [HideInInspector] public int GameDurationInSecounds;
    [HideInInspector] public int EnemySpawnInterval;

    private void Awake()
    {
        // Changes the Seed of Random function to improve unpredictability.
        Random.InitState(System.DateTime.Now.Second);

        GameDurationInSecounds = 180;
        EnemySpawnInterval = 15;

        DontDestroyOnLoad(this.gameObject);
    }

    // Keeps this manager active so the objects in game can use its data.
    private void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
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
    }
}
