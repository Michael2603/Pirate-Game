using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    private UIManager _uiManager; 
    private int _scoreAmount;

    private void Awake()
    {
        // Changes the Seed of Random function to improve unpredictability.
        Random.InitState(System.DateTime.Now.Second);

        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Changes the total score and updates the UI to match the current score.
    public void UpdateScore(int amount)
    {
        _scoreAmount += amount;

        if (_scoreAmount < 0)
        {
            _scoreAmount = 0;
        }

        _uiManager.UpdateUIScore(amount);
    }

    public void UpdatePlayerCurrentAmmo(int ammoAmount)
    {
        _uiManager.UpdateAmmoIcons(ammoAmount);
    }
}
