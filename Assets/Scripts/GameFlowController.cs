using System.Collections;
using UnityEngine;

public class GameFlowController : MonoBehaviour
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
        StartCoroutine(StartRound());
    }

    private IEnumerator StartRound()
    {
        betArea.SetActive(false);
        yield return StartCoroutine(PayoutPhase());

        playerHand.ResetHand();
        dealerHand.ResetHand();

        playerHandUI.ClearHand();
        dealerHandUI.ClearHand();

        deck.InitDeck();
        deck.Shuffle();

        yield return StartCoroutine(deck.DealCard(DealTarget.Player, true));
        yield return StartCoroutine(deck.DealCard(DealTarget.Dealer, true));
        yield return StartCoroutine(deck.DealCard(DealTarget.Player, true));
        yield return StartCoroutine(deck.DealCard(DealTarget.Dealer, false));

        EnablePlayerActionButtons();
    }

    private IEnumerator PayoutPhase()
    {
        yield return StartCoroutine(chipSystem.PlayPayoutAnimation());
    }

    public void StartDealerTurn()
    {
        StartCoroutine(DealerTurnCoroutine());
    }

    private IEnumerator DealerTurnCoroutine()
    {
        DisablePlayerActionButtons();
        standButton.SetActive(false);

        dealerHandUI.cardViews[1].Flip(dealerHand.cards[1]);
        deck.PlayFlipSFX();

        yield return new WaitForSeconds(0.2f);

        while (dealerHand.CalculateHandValue() < 17)
        {
            yield return StartCoroutine(deck.DealCard(DealTarget.Dealer, true));
            yield return new WaitForSeconds(0.2f);
        }

        StartCoroutine(CheckWinner());
    }

    private IEnumerator CheckWinner()
    {
        startButton.SetActive(true);

        if(dealerHand.handType == HandType.Blackjack)
            yield return StartCoroutine(chipSystem.PayInsurance());

        if (isSurrendered)
        {
            yield return StartCoroutine(chipSystem.Surrender());
            isSurrendered = false;
            yield return null;
        }

        HandType playerType = playerHand.handType;
        HandType dealerType = dealerHand.handType;

        if (playerType == HandType.Bust)
            StartCoroutine(chipSystem.LoseBet());
        else if (dealerType == HandType.Bust)
            StartCoroutine(chipSystem.WinBet());
        else if(playerType == HandType.Blackjack)
        {
            if (dealerType == HandType.Blackjack)
                yield return StartCoroutine(chipSystem.Push());
            else
                yield return StartCoroutine(chipSystem.WinBet());
        }
        else if (dealerType == HandType.Blackjack)
            yield return StartCoroutine(chipSystem.LoseBet());
        else
        {
            int playerValue = playerHand.CalculateHandValue();
            int dealerValue = dealerHand.CalculateHandValue();

            if (playerValue > dealerValue)
                yield return StartCoroutine(chipSystem.WinBet());
            else if (playerValue < dealerValue)
                yield return StartCoroutine(chipSystem.LoseBet());
            else
                yield return StartCoroutine(chipSystem.Push());
        }
    }

    public void OnHitButton()
    {
        StartCoroutine(PlayerHit());
    }

    public IEnumerator PlayerHit()
    {
        if (playerHand.CalculateHandValue() > 21)
            yield break;

        yield return StartCoroutine(deck.DealCard(DealTarget.Player, true));
    }

    public void OnDoubleDownButton()
    {
        StartCoroutine(DoubleDown());
    }

    private IEnumerator DoubleDown()
    {
        if (chipSystem.playerChip < chipSystem.currentBet)
            yield break;

        chipSystem.DoubleDown();
        yield return null;

        yield return StartCoroutine(PayoutPhase());

        yield return StartCoroutine(PlayerHit());

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
        StartCoroutine(PayoutPhase());
        insuranceArea.SetActive(false);
    }
}
