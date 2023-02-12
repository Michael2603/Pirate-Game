using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    private int _scoreAmount;

    void Awake()
    {
        // Changes the Seed of Random function to improve unpredictability.
        Random.InitState(System.DateTime.Now.Second);
    }

    // Changes the total score and updates the UI to match the current score.
    public void UpdateScore(int amount)
    {
        _scoreAmount += amount;

        if (_scoreAmount < 0)
        {
            _scoreAmount = 0;
        }

        _scoreText.SetText("Pontuação: " + _scoreAmount);
    }
}
