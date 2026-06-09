using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string savePath;

    private void Awake()
    {
        Instance = this;

        savePath = Path.Combine(Application.persistentDataPath, "stats.json");
    }

    private void Start()
    {
        SaveManager.Instance.LoadStats();
    }

    public void SaveStats()
    {
        StatsData data = new StatsData();

        StatsManager stats = StatsManager.Instance;

        data.totalWins = stats.totalWins;
        data.totalLosses = stats.totalLosses;
        data.totalRounds = stats.totalRounds;
        data.blackjackCount = stats.blackjackCount;
        data.surrenderCount = stats.surrenderCount;

        data.maxWinStreak = stats.maxWinStreak;

        data.maxMoney = stats.maxMoney;

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);
    }

    public void LoadStats()
    {
        if (!File.Exists(savePath))
        {
            return;
        }

        string json = File.ReadAllText(savePath);

        StatsData data = JsonUtility.FromJson<StatsData>(json);

        StatsManager stats = StatsManager.Instance;

        stats.totalWins = data.totalWins;
        stats.totalLosses = data.totalLosses;
        stats.totalRounds = data.totalRounds;
        stats.blackjackCount = data.blackjackCount;
        stats.surrenderCount = data.surrenderCount;

        stats.maxWinStreak = data.maxWinStreak;

        stats.maxMoney = data.maxMoney;
    }

    public void OnApplicationQuit()
    {
        SaveStats();
    }
}