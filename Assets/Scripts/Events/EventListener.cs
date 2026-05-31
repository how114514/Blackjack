using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField] private GameEventSO gameEvent;
    [SerializeField] private UnityEvent response;

    private void OnEnable()
    {
        if (gameEvent == null)
            return;

        gameEvent.Raised += OnEventRaised;
        gameEvent.Register(this);
    }

    private void OnDisable()
    {
        if (gameEvent == null)
            return;

        gameEvent.Raised -= OnEventRaised;
        gameEvent.Unregister(this);
    }

    private void OnEventRaised()
    {
        response?.Invoke();
    }
}