using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    public int totalWins;
    public int totalLosses;
    public int totalRounds;
    public int blackjackCount;
    public int surrenderCount;

    public int currentWinStreak;
    public int maxWinStreak;

    public float WinRate => totalRounds > 0 ? (float)totalWins / totalRounds : 0f;

    private void Awake()
    {
        Instance = this;
    }

    public void AddWin()
    {
        totalWins++;
        totalRounds++;

        currentWinStreak++;

        if (currentWinStreak > maxWinStreak)
            maxWinStreak = currentWinStreak;
    }

    public void AddLoss()
    {
        totalLosses++;
        totalRounds++;

        currentWinStreak = 0;
    }

    public void AddPush()
    {
        totalRounds++;
    }

    public void AddBlackjack()
    {
        blackjackCount++;
    }

    public void AddSurrender()
    {
        surrenderCount++;

        AddLoss();
    }
}
