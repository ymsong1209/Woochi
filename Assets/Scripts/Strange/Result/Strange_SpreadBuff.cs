using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/StrangeResult/SpreadBuff")]
public class Strange_SpreadBuff : StrangeResult
{
    [SerializeField] private GameObject buffObject;

    public override void ApplyEffect()
    {
        MainCharacter mainCharacter = BattleManager.GetInstance.Allies.GetWoochi();
        if(mainCharacter)
        {
            GameObject statObject = Instantiate(buffObject, mainCharacter.transform);
            BaseBuff buff = statObject.GetComponent<BaseBuff>();
            mainCharacter.ApplyBuff(mainCharacter, mainCharacter, buff);
        }
    }
}
