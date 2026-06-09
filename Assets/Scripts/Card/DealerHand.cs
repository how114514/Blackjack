using TMPro;
using UnityEngine;

public class DealerHand : Hand
{
    [SerializeField] private TMP_Text scoreText;

    public void RefreshScore()
    {
        int value = CalculateHandValue();

        scoreText.text = value.ToString();
    }
}