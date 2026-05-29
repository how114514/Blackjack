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

    [Header("sound")]
    [SerializeField] private SFXManager sFX;

    private List<CardDataSO> deck = new();

    //初始化牌堆
    public void InitDeck()
    {
        deck.Clear();
        deck.AddRange(allCards);
    }

    //洗牌
    public void Shuffle()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int rand = Random.Range(i, deck.Count);

            (deck[i], deck[rand]) = (deck[rand], deck[i]);
        }
    }

    //发牌
    public IEnumerator DealCard(DealTarget target, bool faceUp)
    {
        CardDataSO card = DrawCard();

        if (card == null)
            yield break;

        Hand hand = null;
        HandUI handUI = null;
        Transform area = null;

        switch (target)
        {
            case DealTarget.Player:
                hand = playerHand;
                handUI = playerHandUI;
                area = playerArea;
                break;
            case DealTarget.Dealer:
                hand = dealerHand;
                handUI = dealerHandUI;
                area = dealerArea;
                break;
        }

        CardView cardView = Instantiate(cardPrefab, area);
        RectTransform rect = cardView.transform as RectTransform;

        hand.AddCard(card);
        handUI.cardViews.Add(cardView);

        sFX.PlayDeal();
        rect.position = cardDeckArea.position;
        handUI.LayoutAll();
        yield return StartCoroutine(cardView.RotateAnimation());

        if (faceUp)
        {
            PlayFlipSFX();
            cardView.Flip(card);
        }
    }

    //翻牌音效
    public void PlayFlipSFX()
    {
        sFX.PlayFlip();
    }

    //抽牌
    private CardDataSO DrawCard()
    {
        if(deck.Count == 0)
            return null;

        CardDataSO cardData = deck[0];
        deck.RemoveAt(0);

        return cardData;
    }
}
