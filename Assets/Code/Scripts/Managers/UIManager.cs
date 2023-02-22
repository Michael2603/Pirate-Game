using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _matchTimerText;
    [SerializeField] private TMP_Text _endGameScoreText;
    [SerializeField] private List<Image> cannonBallsIcons = new List<Image>();

    private GameplayManager _gameplayManager;

    public GameObject EndGameUI;
    public GameObject GameHUD;


    private void Awake()
    {
        _gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
    }

    private void Update()
    {
        _matchTimerText.text = "Tempo restante:\n" + (int)_gameplayManager.MatchTimer / 60 + ":" + (int)_gameplayManager.MatchTimer % 60;
    }

    // Updates the UI text to match the current score.
    public void UpdateUIScore(int amount)
    {
        _scoreText.SetText("Pontuação: " + amount);
    }

    // Changes the transparency of cannonballs in the UI to show the player their current amount of ammo.
    public void UpdateAmmoIcons(int currentAmmo)
    {
        int index = 0;
        Color tempColor = Color.white;

        foreach(Image icon in cannonBallsIcons)
        {
            if (index == currentAmmo)
            {
                tempColor.a = .2f;
            }
            else
            {
                tempColor.a = 1;
                index++;
            }

            icon.color = tempColor;
        }
    }

    // Hides the HUD and displays the end game screen.
    public void ShowEndGameMenu(int finalScore)
    {
        GameHUD.SetActive(false);
        EndGameUI.SetActive(true);

        _endGameScoreText.text = "Pontuação final:" + finalScore;
    }

    // Used for the endgame button.
    public void ReturnToMenu()
    {
        Destroy(_gameplayManager.gameObject);
        SceneManager.LoadScene(0);
    }
}
