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
        Random.InitState(System.DateTime.Now.Second);
    }

    public void Score(int amount)
    {
        _scoreAmount += amount;

        _scoreText.SetText("Pontuação: " + _scoreAmount);
    }
}
