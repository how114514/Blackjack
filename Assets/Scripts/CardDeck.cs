using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    [SerializeField] private List<CardDataSO> allCards;
    [SerializeField] private Transform playerArea;
    [SerializeField] private Transform dealerArea;
    [SerializeField] private CardView cardPrefab;
    [SerializeField] private PlayerHandUI playerHandUI;
    [SerializeField] private DealerHandUI dealerHandUI;

    private List<CardDataSO> deck = new();

    private void Start()
    {
        InitDeck();
        Shuffle();
    }

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

    [ContextMenu("DealToPlayer")]
    public void DealToPlayer()
    {
        CardDataSO card = DrawCard();
        if (card == null) 
            return;

        CardView cardView = Instantiate(cardPrefab, playerArea);

        cardView.Init(card);

        playerHandUI.AddCard(cardView.transform as RectTransform);
    }

    [ContextMenu("DealToDealer")]
    public void DealToDealer()
    {
        CardDataSO card = DrawCard();
        if (card == null)
            return;

        CardView cardView = Instantiate(cardPrefab, dealerArea);

        cardView.Init(card);

        dealerHandUI.AddCard(cardView.transform as RectTransform);
    }

    public CardDataSO DrawCard()
    {
        if(deck.Count == 0)
            return null;

        CardDataSO cardData = deck[0];

        deck.RemoveAt(0);

        return cardData;
    }
}
