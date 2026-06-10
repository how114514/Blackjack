using TMPro;
using UnityEngine;

public class PlayerHand : Hand
{
    [SerializeField] private GameEventSO playerBlackjackEvent;

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
            ScreenFlash.instance.BustFlash();
        }
        else
            handType = HandType.Normal;
    }
}