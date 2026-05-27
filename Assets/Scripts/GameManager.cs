using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private PlayerHandUI playerHandUI;
    [SerializeField] private DealerHandUI dealerHandUI;

    [Header("Card")]
    [SerializeField] private CardDeck deck;
    [SerializeField] private PlayerHand playerHand;
    [SerializeField] private DealerHand dealerHand;

    [Header("Button")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject hitButton;
    [SerializeField] private GameObject doubleDownButton;
    [SerializeField] private GameObject standButton;
    [SerializeField] private GameObject insuranceButton;
    [SerializeField] private GameObject surrenderButton;

    [Header("Area")]
    [SerializeField] private GameObject betArea;
    [SerializeField] private GameObject insuranceArea;


    [Header("Chip")]
    [SerializeField] private ChipSystem chipSystem;

    private bool isSurrendered;

    private void Start()
    {
        betArea.SetActive(false);
        insuranceArea.SetActive(false);

        DisablePlayerActionButtons();
        standButton.SetActive(false);
    }

    public void ConfirmBet()
    {
        StartCoroutine(DealOpeningCards());
    }

    public IEnumerator DealOpeningCards()
    {
        betArea.SetActive(false);

        playerHand.ClearHand();
        playerHand.NewTurn();
        dealerHand.ClearHand();
        dealerHand.NewTurn();

        playerHandUI.ClearHand();
        dealerHandUI.ClearHand();

        deck.InitDeck();
        deck.Shuffle();

        StartCoroutine(deck.DealToPlayer(1, true));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(deck.DealToDealer(1, true));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(deck.DealToPlayer(1, true));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(deck.DealToDealer(1, false));

        EnablePlayerActionButtons();
    }

    public void StartDealerTurn()
    {
        DisablePlayerActionButtons();
        standButton.SetActive(false);

        dealerHandUI.cardViews[1].Flip(dealerHand.cards[1]);

        while (dealerHand.CalculateHandValue() < 17)
            deck.DealToDealer(1, true);

        CheckWinner();
    }

    private void CheckWinner()
    {
        startButton.SetActive(true);

        if(dealerHand.handType == HandType.Blackjack)
        {
            chipSystem.PayInsurance();
        }

        if (isSurrendered)
        {
            chipSystem.Surrender();
            isSurrendered = false;
            return;
        }

        HandType playerType = playerHand.handType;
        HandType dealerType = dealerHand.handType;

        if (playerType == HandType.Bust)
            chipSystem.LoseBet();
        else if (dealerType == HandType.Bust)
            chipSystem.WinBet();
        else if(playerType == HandType.Blackjack)
        {
            if (dealerType == HandType.Blackjack)
                chipSystem.Push();
            else
                chipSystem.WinBet();
        }
        else if (dealerType == HandType.Blackjack)
            chipSystem.LoseBet();
        else
        {
            int playerValue = playerHand.CalculateHandValue();
            int dealerValue = dealerHand.CalculateHandValue();

            if (playerValue > dealerValue)
                chipSystem.WinBet();
            else if (playerValue < dealerValue)
                chipSystem.LoseBet();
            else
                chipSystem.Push();
        }
    }

    public void PlayerHit()
    {
        if (playerHand.CalculateHandValue() > 21)
            return;

        StartCoroutine(deck.DealToPlayer(1, true));
    }

    public void DoubleDown()
    {
        if(chipSystem.playerChip < chipSystem.currentBet)
            return;

        chipSystem.DoubleDown();

        PlayerHit();

        StartDealerTurn();
    }

    public void OpenBettingArea()
    {
        betArea.SetActive(true);

        startButton.SetActive(false);
    }

    public void Surrender()
    {
        isSurrendered = true;

        DisablePlayerActionButtons();
        standButton.SetActive(false);

        CheckWinner();
    }

    public void Insurance()
    {
        if (dealerHand.cards[0].cardRank != Rank.Ace)
            return;

        insuranceArea.SetActive(true);
    }

    public void DisablePlayerActionButtons()
    {
        hitButton.SetActive(false);
        doubleDownButton.SetActive(false);
        insuranceButton.SetActive(false);
        surrenderButton.SetActive(false);
    }

    private void EnablePlayerActionButtons()
    {
        hitButton.SetActive(true);
        doubleDownButton.SetActive(true);
        standButton.SetActive(true);
        insuranceButton.SetActive(true);
        surrenderButton.SetActive(true);
    }

    public void HideInsuranceArea()
    {
        insuranceArea.SetActive(false);
    }
}
