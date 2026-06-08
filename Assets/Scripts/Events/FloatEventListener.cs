using UnityEngine;
using UnityEngine.Events;

public class FloatEventListener : MonoBehaviour
{
    [SerializeField] private FloatEventSO gameEvent;
    [SerializeField] private UnityEvent<float> response;

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

    private void OnEventRaised(float value)
    {
        response?.Invoke(value);
    }
}