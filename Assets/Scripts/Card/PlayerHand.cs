using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : Hand
{
    [SerializeField] private GameEventSO playerBlackjackEvent;
    [SerializeField] private GameEventSO playerBustEvent;
    
    public override void EvaluateHand()
    {
        int value = CalculateHandValue();

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