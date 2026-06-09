using System;

[Serializable]
public class StatsData
{
    public int totalWins;
    public int totalLosses;
    public int totalRounds;
    public int blackjackCount;
    public int surrenderCount;

    public int maxWinStreak;

    public int maxMoney;
}