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
    [SerializeField] private Button doubleDownButton;
    [SerializeField] private Button standButton;
    [SerializeField] private Button insuranceButton;
    [SerializeField] private Button surrenderButton;

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
    }

    public void DealOpeningCards()
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

        deck.DealToPlayer(1);
        deck.DealToDealer(1);
        deck.DealToPlayer(1);
        deck.DealToDealer(1);
        EnablePlayerActionButtons();
    }

    public void StartDealerTurn()
    {
        DisablePlayerActionButtons();

        while (dealerHand.CalculateHandValue() < 17)
            deck.DealToDealer(1);

        CheckWinner();
    }

    private void CheckWinner()
    {
        startButton.interactable = true;

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
        deck.DealToPlayer(1);
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

        startButton.interactable = false;
    }

    public void Surrender()
    {
        isSurrendered = true;

        DisablePlayerActionButtons();
        CheckWinner();
    }

    public void Insurance()
    {
        if (dealerHand.cards[0].cardRank != Rank.Ace)
            return;

        insuranceArea.SetActive(true);
    }

    private void DisablePlayerActionButtons()
    {
        hitButton.interactable = false;
        doubleDownButton.interactable = false;
        standButton.interactable = false;
        insuranceButton.interactable = false;
        surrenderButton.interactable = false;
    }

    private void EnablePlayerActionButtons()
    {
        hitButton.interactable = true;
        standButton.interactable = true;
        doubleDownButton.interactable = true;
        surrenderButton.interactable = true;
        insuranceButton.interactable = true;
    }

    public void HideInsuranceArea()
    {
        insuranceArea.SetActive(false);
    }
}
