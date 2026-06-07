using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public TMP_Text totalWinsText;
    public TMP_Text totalLossesText;
    public TMP_Text totalRoundsText;
    public TMP_Text winRateText;
    public TMP_Text blackjackCountText;
    public TMP_Text surrenderCountText;
    public TMP_Text maxWinStreakText;
    public TMP_Text maxMoneyText;

    private StatsManager stats;

    private void Awake()
    {
        stats = StatsManager.Instance;
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        totalWinsText.text = "胜利回合数：" + stats.totalWins;
        totalLossesText.text = "失败回合数：" + stats.totalLosses;
        totalRoundsText.text = "总回合数：" + stats.totalRounds;

        blackjackCountText.text = "Blackjack 次数：" + stats.blackjackCount;
        surrenderCountText.text = "投降次数：" + stats.surrenderCount;
        maxWinStreakText.text = "最大连胜：" + stats.maxWinStreak;

        float winRate = 0f;
        if (stats.totalRounds > 0)
        {
            winRate = (float)stats.totalWins / stats.totalRounds * 100f;
        }
        winRateText.text = "胜率：" + winRate.ToString("F1") + "%";

        maxMoneyText.text = "最大金额：" + stats.maxMoney;
    }
}
