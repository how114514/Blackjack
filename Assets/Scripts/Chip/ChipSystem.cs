using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChipSystem : MonoBehaviour
{
    public int playerChip;
    public int currentBet;
    public int currentInsuranceBet;

    public ChipViewManager chipViewManager;
    [SerializeField] private SFXManager sFX;
    [SerializeField] private PlayerHand playerHand;
    [SerializeField] private TMP_Text playerChipText;

    [Header("Chip Data")]
    [SerializeField] private ChipDataSO chip1;
    [SerializeField] private ChipDataSO chip5;
    [SerializeField] private ChipDataSO chip10;
    [SerializeField] private ChipDataSO chip25;
    [SerializeField] private ChipDataSO chip100;
    [SerializeField] private ChipDataSO chip500;
    [SerializeField] private ChipDataSO chip1000;
    [SerializeField] private ChipDataSO chip5000;
    private Dictionary<int, ChipDataSO> chipMap;

    public List<ChipDataSO> chipList = new();
    private readonly int[] chipValues =
    {
        5000, 1000, 500, 100,
        25, 10, 5, 1
    };

    private void Awake()
    {
        chipMap = new Dictionary<int, ChipDataSO>()
        {
            { 1, chip1 }, { 5, chip5 },
            { 10, chip10 }, { 25, chip25 },
            { 100, chip100 }, { 500, chip500 },
            { 1000, chip1000 }, { 5000, chip5000 }
        };
    }

    //根据玩家的手牌类型计算赢得的金额，如果是Blackjack则按照2.5倍支付，否则按照2倍支付
    private int CalculatePayout()
    {
        if (playerHand.handType == HandType.Blackjack)
            return Mathf.RoundToInt(currentBet * 2.5f);
        else
            return currentBet * 2;
    }

    //根据指定的金额构建一个包含相应筹码数据的列表，按照从大到小的顺序添加筹码，直到剩余金额为0
    private List<ChipDataSO> BuildChipList(int amount)
    {
        List<ChipDataSO> list = new();

        int remaining = amount;

        foreach (int value in chipValues)
        {
            while (remaining >= value)
            {
                list.Add(chipMap[value]);

                remaining -= value;
            }
        }

        return list;
    }

    //处理玩家赢得的金额，首先根据金额构建筹码数据列表，然后调用ChipViewManager生成收入筹码的动画，最后更新玩家筹码数量并播放文本动画
    public IEnumerator WinBet()
    {
        int amount = CalculatePayout();
        yield return StartCoroutine(ResolveIncome(amount));
    }

    //处理玩家输掉的金额，重置当前投注和保险投注为0
    public IEnumerator LoseBet()
    {
        currentBet = 0;
        currentInsuranceBet = 0;
        yield return null;
    }

    //处理玩家平局的情况，首先根据当前投注金额构建筹码数据列表，然后调用ChipViewManager生成收入筹码的动画，最后更新玩家筹码数量并播放文本动画
    public IEnumerator Push()
    {
        int amount = currentBet;
        yield return StartCoroutine(ResolveIncome(amount));
    }

    //处理玩家投降的情况，首先计算玩家应该退回的金额（当前投注的一半），然后调用ResolveIncome方法处理收入动画和筹码数量更新
    public IEnumerator Surrender()
    {
        int amount = Mathf.RoundToInt(currentBet * 0.5f);
        yield return StartCoroutine(ResolveIncome(amount));
    }

    //处理玩家赢得保险投注的情况，首先计算玩家应该获得的金额（当前保险投注的三倍），然后调用ResolveIncome方法处理收入动画和筹码数量更新
    public IEnumerator PayInsurance()
    {
        int amount = currentInsuranceBet * 3;
        yield return StartCoroutine(ResolveIncome(amount));
    }

    //处理玩家赢得的金额，首先根据金额构建筹码数据列表，然后调用ChipViewManager生成收入筹码的动画，最后更新玩家筹码数量并播放文本动画
    private IEnumerator ResolveIncome(int amount)
    {
        List<ChipDataSO> chips = BuildChipList(amount);

        yield return StartCoroutine(chipViewManager.SpawnIncomeChips(chips));

        int oldValue = playerChip;

        playerChip += amount;

        currentBet = 0;
        currentInsuranceBet = 0;

        yield return StartCoroutine(AnimateChipText(oldValue, playerChip));
    }

    //调整玩家的投注金额，首先检查玩家是否有足够的筹码进行调整，以及调整后的投注金额是否有效，然后更新当前投注和玩家筹码数量，并刷新筹码UI以反映新的投注状态
    public void AdjustBet(int amount)
    {
        if (playerChip < amount)
            return;

        if (currentBet + amount < 0)
            return;

        currentBet += amount;
        playerChip -= amount;


        OnChipCommitted(currentBet);
    }

    //将玩家的所有筹码投注到当前投注中，首先将当前投注增加玩家的筹码数量，然后将玩家的筹码数量设置为0，最后刷新筹码UI以反映新的投注状态
    public void BetAll()
    {
        currentBet += playerChip;
        playerChip = 0;

        OnChipCommitted(currentBet);
    }

    //取消当前的投注，将当前投注金额退回玩家的筹码数量，并将当前投注重置为0，最后刷新筹码UI以反映新的投注状态
    public void ClearBet()
    {
        playerChip += currentBet;
        currentBet = 0;

        OnChipCommitted(currentBet);
    }

    //调整玩家的保险投注金额，首先检查玩家是否有足够的筹码进行调整，以及调整后的保险投注金额是否有效（不能为负数且不能超过当前投注的一半），然后更新当前保险投注和玩家筹码数量，并刷新筹码UI以反映新的保险投注状态
    public void AdjustInsurance(int amount)
    {
        if (playerChip < amount)
            return;

        if (currentInsuranceBet + amount < 0)
            return;

        if(currentInsuranceBet + amount > currentBet / 2)
            return;

        currentInsuranceBet += amount;
        playerChip -= amount;

        OnChipCommitted(currentInsuranceBet);
    }

    //将玩家的保险投注调整到最大值（当前投注的一半），首先计算需要增加的保险投注金额，然后更新当前保险投注和玩家筹码数量，并刷新筹码UI以反映新的保险投注状态
    public void MaxInsurance()
    {
        playerChip -= currentBet / 2 - currentInsuranceBet;
        currentInsuranceBet = currentBet / 2;

        OnChipCommitted(currentInsuranceBet);
    }

    //取消当前的保险投注，将当前保险投注金额退回玩家的筹码数量，并将当前保险投注重置为0，最后刷新筹码UI以反映新的保险投注状态
    public void CancelInsurance()
    {
        playerChip += currentInsuranceBet;
        currentInsuranceBet = 0;

        OnChipCommitted(currentInsuranceBet);
    }

    //将玩家的当前投注金额加倍，首先检查玩家是否有足够的筹码进行加倍，然后将当前投注增加到原来的两倍，并将玩家的筹码数量减少相应的金额，最后刷新筹码UI以反映新的投注状态
    public void DoubleDown()
    {
        int addAmount = currentBet;
        playerChip -= addAmount;
        currentBet += addAmount;

        OnChipCommitted(addAmount);
    }

    //调用ChipViewManager的PayoutAnimation方法来播放支付筹码的动画效果
    public IEnumerator PlayPayoutAnimation()
    {
        yield return StartCoroutine(chipViewManager.PayoutAnimation());
    }

    //当玩家的投注或保险投注发生变化时调用，首先根据新的投注金额构建筹码数据列表，然后调用ChipViewManager刷新筹码视图以反映新的投注状态，最后更新玩家筹码数量的文本显示
    private void OnChipCommitted(int amount)
    {
        sFX.PlayChip();

        chipList = BuildChipList(amount);
        StartCoroutine(chipViewManager.RefreshChipView(chipList));
        playerChipText.text = playerChip.ToString();
    }

    //使用线性插值在一定时间内平滑地将筹码数量从初始值过渡到目标值，并在过渡过程中更新UI文本显示
    public IEnumerator AnimateChipText(int from, int to)
    {
        float duration = 1f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            int value = Mathf.RoundToInt(Mathf.Lerp(from, to, lerp));
            playerChipText.text = value.ToString();

            yield return null;
        }

        playerChipText.text = to.ToString();
    }
}
