using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/StrangeResult/StatBuff")]
public class Strange_StatBuff : StrangeResult
{
    [SerializeField] private GameObject buffObject;
    [SerializeField] private bool onlyWoochi;

    public override void ApplyEffect()
    {
        if (onlyWoochi)
        {
            MainCharacter mainCharacter = BattleManager.GetInstance.Allies.GetWoochi();
            if(mainCharacter)
            {
                GameObject statObject = Instantiate(buffObject, mainCharacter.transform);
                StatBuff buff = statObject.GetComponent<StatBuff>();
                mainCharacter.ApplyBuff(mainCharacter, mainCharacter, buff);
            }
        }
        else
        {
            var allCharacter = BattleManager.GetInstance.Allies.AllCharacter;
            allCharacter.RemoveAll(c => c.IsDead);

            foreach (BaseCharacter character in allCharacter)
            {
                GameObject statObject = Instantiate(buffObject, character.transform);
                StatBuff buff = statObject.GetComponent<StatBuff>();
                character.ApplyBuff(character, character, buff);
            }
        }
    }
}
