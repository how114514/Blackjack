using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Button startButton;
    [SerializeField] private Button hitButton;
    [SerializeField] private Button standButton;
    [SerializeField] private List<Button> betButtons;

    [Header("Chip")]
    [SerializeField] private ChipSystem chipSystem;

    private void Start()
    {
        DisableBettingUI();
        hitButton.interactable = false;
        standButton.interactable = false;
    }

    public void DealOpeningCards()
    {
        DisableBettingUI();

        playerHand.ClearHand();
        playerHand.NewTurn();
        dealerHand.ClearHand();

        playerHandUI.ClearHand();
        dealerHandUI.ClearHand();

        deck.InitDeck();
        deck.Shuffle();

        deck.DealToPlayer(1);
        deck.DealToDealer(1);
        deck.DealToPlayer(1);
        deck.DealToDealer(1);

        hitButton.interactable = true;
        standButton.interactable = true;
    }

    public void StartDealerTurn()
    {
        hitButton.interactable = false;
        standButton.interactable = false;

        while (dealerHand.CalculateHandValue() < 17)
            deck.DealToDealer(1);

        CheckWinner();
    }

    private void CheckWinner()
    {
        if (playerHand.CalculateHandValue() > 21)
            chipSystem.LoseBet();
        else
        {
            if (dealerHand.CalculateHandValue() > 21)
                chipSystem.WinBet();
            else
            {
                if (playerHand.CalculateHandValue() > dealerHand.CalculateHandValue())
                    chipSystem.WinBet();
                else if (playerHand.CalculateHandValue() < dealerHand.CalculateHandValue())
                    chipSystem.LoseBet();
                else
                    chipSystem.Push();
            }
        }

        startButton.interactable = true;
    }

    public void PlayerHit()
    {
        deck.DealToPlayer(1);

        if (playerHand.isBust)
            hitButton.interactable = false;
    }

    public void StartBetting()
    {
        EnableBettingUI();
        startButton.interactable = false;
    }

    private void EnableBettingUI()
    {
        foreach (Button button in betButtons)
        {
            button.interactable = true;
        }
    }

    private void DisableBettingUI()
    {
        foreach (Button button in betButtons)
        {
            button.interactable = false;
        }
    }
}
