using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/StrangeResult/Ghost/Ignore")]
public class Strange_Ghost_Ignore : StrangeResult
{
    
    public override void ApplyEffect()
    {
        base.ApplyEffect();
        
        // 다음 보상 희귀도 하나 감소
    }
}
