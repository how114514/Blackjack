using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    [SerializeField] private List<CardDataSO> allCards;

    [Header("Instantiate")]
    [SerializeField] private Transform playerArea;
    [SerializeField] private Transform dealerArea;
    [SerializeField] private RectTransform cardDeckArea;
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

    public IEnumerator DealToPlayer(int count, bool faceUp)
    {
        for (int i = 0; i < count; i++)
        {
            CardDataSO card = DrawCard();
            if (card == null)
                yield return null;

            CardView cardView = Instantiate(cardPrefab, playerArea);
            RectTransform rect = cardView.transform as RectTransform;

            playerHand.AddCard(card);
            playerHandUI.cardViews.Add(cardView);

            rect.position = cardDeckArea.position;

            playerHandUI.LayoutAll();
            StartCoroutine(cardView.RotateAnimation());

            yield return new WaitForSeconds(0.2f);

            if (faceUp)
                cardView.Flip(card);
        }
    }

    public IEnumerator DealToDealer(int count, bool faceUp)
    {
        for (int i = 0; i < count; i++)
        {
            CardDataSO card = DrawCard();
            if (card == null)
                yield return null;

            CardView cardView = Instantiate(cardPrefab, dealerArea);
            RectTransform rect = cardView.transform as RectTransform;

            dealerHand.AddCard(card);
            dealerHandUI.cardViews.Add(cardView);

            rect.position = cardDeckArea.position;

            dealerHandUI.LayoutAll();
            StartCoroutine(cardView.RotateAnimation());

            yield return new WaitForSeconds(0.2f);

            if (faceUp)
                cardView.Flip(card);
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
