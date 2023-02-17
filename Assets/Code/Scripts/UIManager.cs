using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private List<Image> cannonBallsIcons = new List<Image>();

    // Changes the total score and updates the UI to match the current score.
    public void UpdateUIScore(int amount)
    {
        _scoreText.SetText("Pontuação: " + amount);
    }

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
}
