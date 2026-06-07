using UnityEngine;
using UnityEngine.Events;

public class IntEventListener : MonoBehaviour
{
    [SerializeField] private IntEventSO gameEvent;
    [SerializeField] private UnityEvent<int> response;

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

    private void OnEventRaised(int value)
    {
        response?.Invoke(value);
    }
}