using UnityEngine;

[CreateAssetMenu(fileName = "Strange_", menuName = "Scriptable Objects/StrangeResult/StatDeBuff")]
public class Strange_StatDebuff : StrangeResult
{
    [SerializeField] private GameObject buffObject;
    [SerializeField] private bool onlyWoochi;

    public override void ApplyEffect()
    {
        if (onlyWoochi)
        {
            MainCharacter mainCharacter = BattleManager.GetInstance.Allies.GetWoochi();
            if (mainCharacter)
            {
                GameObject statObject = Instantiate(buffObject, mainCharacter.transform);
                StatDeBuff buff = statObject.GetComponent<StatDeBuff>();
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
                StatDeBuff buff = statObject.GetComponent<StatDeBuff>();
                character.ApplyBuff(character, character, buff);
            }
        }
    }
}
