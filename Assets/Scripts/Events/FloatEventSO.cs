using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Float Event")]
public class FloatEventSO : ScriptableObject
{
    public event Action<float> Raised;

    private readonly List<FloatEventListener> listeners = new();

    public void Register(FloatEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void Unregister(FloatEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise(float value)
    {
        Raised?.Invoke(value);
    }
}
