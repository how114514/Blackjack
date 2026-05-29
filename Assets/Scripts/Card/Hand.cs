using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<CardDataSO> cards = new();
    public HandType handType = HandType.Normal;

    //计算手牌的总点数，考虑A的特殊情况
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

    //添加一张牌到手牌中，并重新评估手牌类型
    public virtual void AddCard(CardDataSO card)
    {
        cards.Add(card);

        EvaluateHand();
    }

    //评估手牌类型，根据当前的牌和点数来确定是正常、黑杰克还是爆牌
    private void EvaluateHand()
    {
        int value = CalculateHandValue();

        if (cards.Count == 2 && value == 21)
            handType = HandType.Blackjack;
        else if (value > 21)
            handType = HandType.Bust;
        else
            handType = HandType.Normal;
    }

    //重置手牌，清空牌列表并将手牌类型重置为正常
    public void ResetHand()
    {
        cards.Clear();
        handType = HandType.Normal;
    }
}