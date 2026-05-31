using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Game Event")]
public class GameEventSO : ScriptableObject
{
    public event Action Raised;

    private readonly List<EventListener> listeners = new();

    public IReadOnlyList<EventListener> Listeners => listeners;

    public void Register(EventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void Unregister(EventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise()
    {
        Raised?.Invoke();
    }
}