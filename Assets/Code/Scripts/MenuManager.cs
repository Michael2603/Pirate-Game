using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameplayManager GameplayManager;
    
    public GameObject MainMenuPanel;
    public GameObject ConfigurationsPanel;

    public Slider GameDurationSlider;
    public TMP_Text GameDurationValueText;

    public Slider DifficultySlider;
    public TMP_Text DifficultyValueText;


    public void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void EnterConfigurations()
    {
        MainMenuPanel.SetActive(false);
        ConfigurationsPanel.SetActive(true);
    }

    public void ReturnFromConfigurations()
    {
        ConfigurationsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void AdjustGameDuration()
    {
        int durationInMinutes = (int)Mathf.Round(GameDurationSlider.value)/ 60;
        
        GameplayManager.GameDurationInSecounds = durationInMinutes * 60;

        GameDurationValueText.text = durationInMinutes + " Minutos";
    }

    public void AdjustDifficulty()
    {
        GameplayManager.EnemySpawnInterval = (int)Mathf.Round(DifficultySlider.value);

        DifficultyValueText.text = (int)Mathf.Round(DifficultySlider.value) + " Segundos";
    }
}
