using System.Threading.Tasks;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private GameEventSO startGameEvent;

    public void OnStartGameButtonClicked()
    {
        startGameEvent.Raise();
    }

    public void OnExitGameButtonClicked()
    {
        SaveManager.Instance.SaveStats();

        Application.Quit();
    }
}
