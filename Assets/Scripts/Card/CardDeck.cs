using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    [SerializeField] private List<CardDataSO> allCards;

    [Header("Instantiate")]
    [SerializeField] private Transform playerArea;
    [SerializeField] private Transform dealerArea;
    [SerializeField] private CardView cardPrefab;

    [Header("UI")]
    [SerializeField] private PlayerHandUI playerHandUI;
    [SerializeField] private DealerHandUI dealerHandUI;

    [Header("List")]
    [SerializeField] private PlayerHand playerHand;
    [SerializeField] private DealerHand dealerHand;

    private List<CardDataSO> deck = new();

    public void InitDeck()
    {
        deck.Clear();
        deck.AddRange(allCards);
    }

    public void Shuffle()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int rand = Random.Range(i, deck.Count);

            (deck[i], deck[rand]) = (deck[rand], deck[i]);
        }
    }

    public void DealToPlayer(int count)
    {
        int i;
        for (i = count; i > 0; i--) 
        {
            CardDataSO card = DrawCard();
            if (card == null)
                return;

            playerHand.AddCard(card);

            CardView cardView = Instantiate(cardPrefab, playerArea);

            cardView.Init(card);

            playerHandUI.AddCard(cardView.transform as RectTransform);
        }
    }

    public void DealToDealer(int count)
    {
        int i;
        for (i = count; i > 0; i--)
        {
            CardDataSO card = DrawCard();
            if (card == null)
                return;

            dealerHand.AddCard(card);

            CardView cardView = Instantiate(cardPrefab, dealerArea);

            cardView.Init(card);

            dealerHandUI.AddCard(cardView.transform as RectTransform);
        }
    }

    private CardDataSO DrawCard()
    {
        if(deck.Count == 0)
            return null;

        CardDataSO cardData = deck[0];

        deck.RemoveAt(0);

        return cardData;
    }
}
