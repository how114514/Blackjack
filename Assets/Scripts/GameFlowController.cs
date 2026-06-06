using System.Collections;
using UnityEngine;

public class GameFlowController : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private GameEventSO playerWinEvent;
    [SerializeField] private GameEventSO playerLoseEvent;
    [SerializeField] private GameEventSO pushEvent;
    [SerializeField] private GameEventSO playerSurrenderEvent;
    [SerializeField] private GameEventSO newRoundEvent;
    [SerializeField] private GameEventSO InsuranceLostEvent;
    [SerializeField] private GameEventSO InsuranceWonEvent;
    [SerializeField] private GameEventSO loadMenuEvent;

    [Header("CardUI")]
    [SerializeField] private DealerHandUI dealerHandUI;

    [Header("Card")]
    [SerializeField] private CardDeck deck;
    [SerializeField] private PlayerHand playerHand;
    [SerializeField] private DealerHand dealerHand;

    [Header("UI")]
    [SerializeField] private UIManager uiManager;

    [Header("Chip")]
    [SerializeField] private ChipSystem chipSystem;

    private bool isSurrendered;

    //开始下注阶段
    public void OpenBettingArea()
    {
        uiManager.OpenBettingArea();
    }

    //确认下注，开始游戏
    public void ConfirmBet()
    {
        StartCoroutine(StartRound());
    }

    //开始新的一轮游戏
    private IEnumerator StartRound()
    {
        uiManager.CloseBettingArea();

        yield return StartCoroutine(PayoutPhase());

        newRoundEvent.Raise();

        yield return StartCoroutine(deck.DealCard(DealTarget.Player, true));
        yield return StartCoroutine(deck.DealCard(DealTarget.Dealer, true));
        yield return StartCoroutine(deck.DealCard(DealTarget.Player, true));
        yield return StartCoroutine(deck.DealCard(DealTarget.Dealer, false));

        bool canInsurance = dealerHand.cards[0].cardRank == Rank.Ace;
        uiManager.EnablePlayerActionButtons(canInsurance);
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
        uiManager.DisablePlayerActionButtons();

        yield return StartCoroutine(dealerHandUI.cardViews[1].FlipAnimation(dealerHand.cards[1]));

        while (dealerHand.CalculateHandValue() < 17)
        {
            yield return StartCoroutine(deck.DealCard(DealTarget.Dealer, true));
        }

        ResolveRound();
    }

    //结算当前回合
    private void ResolveRound()
    {
        if (isSurrendered)
        {
            playerSurrenderEvent.Raise();

            isSurrendered = false;
            uiManager.EnableStartButton();
        }

        HandType playerType = playerHand.handType;
        HandType dealerType = dealerHand.handType;

        if (playerType == HandType.Bust)
            playerLoseEvent.Raise();
        else if (dealerType == HandType.Bust)
            playerWinEvent.Raise();
        else if(playerType == HandType.Blackjack)
        {
            if (dealerType == HandType.Blackjack)
                pushEvent.Raise();
            else
                playerWinEvent.Raise();
        }
        else if (dealerType == HandType.Blackjack)
            playerLoseEvent.Raise();
        else
        {
            int playerValue = playerHand.CalculateHandValue();
            int dealerValue = dealerHand.CalculateHandValue();

            if (playerValue > dealerValue)
                playerWinEvent.Raise();
            else if (playerValue < dealerValue)
                playerLoseEvent.Raise();
            else
                pushEvent.Raise();
        }

        uiManager.EnableStartButton();
    }

    //玩家选择要牌
    public void OnHitButton()
    {
        StartCoroutine(PlayerHit());
    }

    //玩家要牌流程
    public IEnumerator PlayerHit()
    {
        uiManager.OnPlayerHit();

        yield return StartCoroutine(deck.DealCard(DealTarget.Player, true));

        if (playerHand.handType == HandType.Bust)
        {
            uiManager.DisablePlayerActionButtons();

            ResolveRound();
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

        if(playerHand.handType != HandType.Bust)
            StartDealerTurn();
    }

    //玩家选择投降
    public void Surrender()
    {
        isSurrendered = true;

        uiManager.DisablePlayerActionButtons();

        ResolveRound();
    }

    //玩家选择保险
    public void Insurance()
    {
        if (dealerHand.cards[0].cardRank != Rank.Ace)
            return;

        uiManager.OpenInsuranceArea();
        uiManager.DisablePlayerActionButtons();
    }

    public void ConfirmInsurance()
    {
        StartCoroutine(ConfirmInsuranceCoroutine());
    }

    //玩家确认购买保险
    private IEnumerator ConfirmInsuranceCoroutine()
    {
        yield return StartCoroutine(PayoutPhase());

        uiManager.CloseInsuranceArea();

        if (dealerHand.handType == HandType.Blackjack)
        {
            yield return StartCoroutine(dealerHandUI.cardViews[1].FlipAnimation(dealerHand.cards[1]));

            InsuranceWonEvent.Raise();

            uiManager.EnableStartButton();
        }
        else
        {
            InsuranceLostEvent.Raise();
            uiManager.EnablePlayerActionButtons(false);
        }
    }

    //显示游戏结束界面
    public void ShowGameOver()
    {
        uiManager.ShowGameOver();
    }

    //返回主菜单
    public void BackToMenu()
    {
        loadMenuEvent.Raise();
    }
}
