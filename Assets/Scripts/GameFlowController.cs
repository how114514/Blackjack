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

    [SerializeField] private SFXManager sFX;

    private bool isSurrendered;
    private bool isInsurancePurchased;

    private void Start()
    {
        betArea.SetActive(false);
        insuranceArea.SetActive(false);

        DisablePlayerActionButtons();
    }

    //开始下注阶段
    public void OpenBettingArea()
    {
        betArea.SetActive(true);

        startButton.SetActive(false);
    }

    //确认下注，开始游戏
    public void ConfirmBet()
    {
        StartCoroutine(StartRound());
    }

    //开始新的一轮游戏
    private IEnumerator StartRound()
    {
        isInsurancePurchased = false;

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

    //播放收走筹码动画
    private IEnumerator PayoutPhase()
    {
        yield return StartCoroutine(chipSystem.PlayPayoutAnimation());
    }

    //开始庄家回合
    public void StartDealerTurn()
    {
        StartCoroutine(DealerTurnCoroutine());
    }

    //庄家回合流程
    private IEnumerator DealerTurnCoroutine()
    {
        DisablePlayerActionButtons();

        yield return StartCoroutine(dealerHandUI.cardViews[1].FlipAnimation(dealerHand.cards[1]));
        deck.PlayFlipSFX();

        while (dealerHand.CalculateHandValue() < 17)
        {
            yield return StartCoroutine(deck.DealCard(DealTarget.Dealer, true));
        }

        yield return StartCoroutine(ResolveRound());
    }

    //结算当前回合
    private IEnumerator ResolveRound()
    {
        if (isSurrendered)
        {
            yield return StartCoroutine(chipSystem.Surrender());
            isSurrendered = false;
            startButton.SetActive(true);

            yield break;
        }

        HandType playerType = playerHand.handType;
        HandType dealerType = dealerHand.handType;

        if (playerType == HandType.Bust)
            yield return StartCoroutine(chipSystem.LoseBet());
        else if (dealerType == HandType.Bust)
            yield return StartCoroutine(chipSystem.WinBet());
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

        startButton.SetActive(true);
    }

    //玩家选择要牌
    public void OnHitButton()
    {
        StartCoroutine(PlayerHit());
    }

    //玩家要牌流程
    public IEnumerator PlayerHit()
    {
        yield return StartCoroutine(deck.DealCard(DealTarget.Player, true));

        if (playerHand.handType == HandType.Bust)
        {
            DisablePlayerActionButtons();

            yield return StartCoroutine(ResolveRound());
        }
    }

    //玩家选择加倍
    public void OnDoubleDownButton()
    {
        StartCoroutine(DoubleDown());
    }

    //玩家加倍流程
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

    //玩家选择投降
    public void Surrender()
    {
        isSurrendered = true;

        DisablePlayerActionButtons();

        StartCoroutine(ResolveRound());
    }

    //玩家选择保险
    public void Insurance()
    {
        if (dealerHand.cards[0].cardRank != Rank.Ace)
            return;

        insuranceArea.SetActive(true);
        DisablePlayerActionButtons();
    }

    public void ConfirmInsurance()
    {
        StartCoroutine(ConfirmInsuranceCoroutine());
    }

    //玩家确认购买保险
    private IEnumerator ConfirmInsuranceCoroutine()
    {
        yield return StartCoroutine(PayoutPhase());

        insuranceArea.SetActive(false);
        isInsurancePurchased = true;

        if (dealerHand.handType == HandType.Blackjack)
        {
            yield return StartCoroutine(dealerHandUI.cardViews[1].FlipAnimation(dealerHand.cards[1]));
            deck.PlayFlipSFX();

            yield return StartCoroutine(chipSystem.PayInsurance());
            startButton.SetActive(true);
        }
        else
        {
            sFX.PlayLose();
            EnablePlayerActionButtons();
        }
    }

    //关闭玩家操作按钮
    public void DisablePlayerActionButtons()
    {
        hitButton.SetActive(false);
        doubleDownButton.SetActive(false); 
        standButton.SetActive(false);
        surrenderButton.SetActive(false);
        insuranceButton.SetActive(false);
    }

    //开启玩家操作按钮
    private void EnablePlayerActionButtons()
    {
        hitButton.SetActive(true);
        doubleDownButton.SetActive(true);
        standButton.SetActive(true);
        surrenderButton.SetActive(true);

        if (dealerHand.cards[0].cardRank == Rank.Ace || !isInsurancePurchased)
            insuranceButton.SetActive(true);
    }
}
