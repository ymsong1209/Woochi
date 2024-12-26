using System;
using OneLine;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor_", menuName = "Scriptable Objects/Plot/Normal")]
public class Plot : ScriptableObject
{
    private Action<int> onComplete;
    
    public ScenarioType type;
    [OneLineWithHeader] public Illustration[] illustrations;
    public Sprite blindImage;
    public Actor speaker;
    [TextArea(3, 10)] public string text;
    public PlotEvent plotEvent = PlotEvent.None;     // 이벤트가 들어올 때 해당 이벤트와 일치해야 함
    
    [Header("Option")] 
    public bool showBattle = false;
    public bool isTextUp = false;
    public bool showText = true;
    public bool isBlind = false;
    
    public void Play(Action<int> callback)
    {
        onComplete = callback;
        
        ScenarioManager.GetInstance.SetUI(this);
        Effect();
    }

    public void Next(PlotEvent plotEvent, int index = -1)
    {
        if (this.plotEvent == plotEvent)
        {
            GameManager.GetInstance.soundManager.PlaySFX("Reward_Arrow_Click");
            onComplete?.Invoke(index);
        }
    }
    
    public virtual void Effect()
    {
        
    }
}

[Serializable]
public struct Illustration
{
    public Actor actor;
    public int direction;   // 0 : left, 1 : center, 2 : right
}