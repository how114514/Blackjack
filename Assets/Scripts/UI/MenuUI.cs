using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject statsPanel;

    public void OpenStatsPanel()
    {
        statsPanel.SetActive(true);
    }

    public void CloseStatsPanel()
    {
        statsPanel.SetActive(false);
    }
}
