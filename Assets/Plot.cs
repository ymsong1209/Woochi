using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Plot_", menuName = "Scriptable Objects/Scenario/Plot")]
public class Plot : ScriptableObject
{
    private Action<int> onComplete;

    [SerializeField, TextArea(3, 10)] protected string dialogue;

    public virtual void Play(Action<int> callback)
    {
        onComplete = callback;
    }

    public void Next(int index = -1)
    {
        onComplete?.Invoke(index);
    }
}
