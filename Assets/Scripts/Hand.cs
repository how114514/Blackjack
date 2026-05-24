using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<CardDataSO> cards = new();

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
    }

    public void ClearHand()
    {
        cards.Clear();
    }
}