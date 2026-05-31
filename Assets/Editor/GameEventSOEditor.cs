using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEventSO))]
public class GameEventSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameEventSO gameEvent = (GameEventSO)target;

        GUILayout.Space(10);

        GUILayout.Label("Runtime Listeners", EditorStyles.boldLabel);

        foreach (var listener in gameEvent.Listeners)
        {
            if (listener != null)
            {
                EditorGUILayout.ObjectField(listener, typeof(EventListener), true);
            }
        }
    }
}