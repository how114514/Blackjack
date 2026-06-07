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
        Application.Quit();
    }
}
