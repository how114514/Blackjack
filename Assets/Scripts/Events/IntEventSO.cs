using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Int Event")]
public class IntEventSO : ScriptableObject
{
    public event Action<int> Raised;

    private readonly List<IntEventListener> listeners = new();

    public void Register(IntEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void Unregister(IntEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise(int value)
    {
        Raised?.Invoke(value);
    }
}
