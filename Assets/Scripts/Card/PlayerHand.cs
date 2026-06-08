using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHand : Hand
{
    [SerializeField] private GameEventSO playerBlackjackEvent;
    [SerializeField] private GameEventSO playerBustEvent;

    [SerializeField] private TMP_Text scoreText;

    //添加一张牌到手牌中，并重新评估手牌类型
    public override void AddCard(CardDataSO card)
    {
        cards.Add(card);

        int value = CalculateHandValue();

        EvaluateHand(value);

        scoreText.text = value.ToString();
    }

    public override void EvaluateHand(int value)
    {
        if (cards.Count == 2 && value == 21)
        { 
            handType = HandType.Blackjack;
            playerBlackjackEvent.Raise();
        }
        else if (value > 21)
        {
            handType = HandType.Bust;
            playerBustEvent.Raise();
        }
        else
            handType = HandType.Normal;
    }
}