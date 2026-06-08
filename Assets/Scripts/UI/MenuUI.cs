using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject settingPanel;

    public void OpenStatsPanel()
    {
        statsPanel.SetActive(true);
    }

    public void CloseStatsPanel()
    {
        statsPanel.SetActive(false);
    }

    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        settingPanel.SetActive(false);
    }
}
