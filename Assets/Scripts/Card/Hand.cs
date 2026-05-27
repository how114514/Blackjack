using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<CardDataSO> cards = new();
    public HandType handType = HandType.Normal;

    public int CalculateHandValue()
    {
        int total = 0;
        int aceCount = 0;

        foreach (CardDataSO card in cards)
        {
            if (card.cardRank == Rank.Ace)
                aceCount++;

            total += card.cardValue;
        }

        if (aceCount >= 1 && total + 10 <= 21)
            total += 10;

        return total;
    }

    public virtual void AddCard(CardDataSO card)
    {
        cards.Add(card);

        EvaluateHand();
    }

    public void ClearHand()
    {
        cards.Clear();
    }

    public void EvaluateHand()
    {
        int value = CalculateHandValue();

        if (cards.Count == 2 && value == 21)
            handType = HandType.Blackjack;
        else if (value > 21)
            handType = HandType.Bust;
        else
            handType = HandType.Normal;
    }

    public void NewTurn()
    {
        handType = HandType.Normal;
    }
}