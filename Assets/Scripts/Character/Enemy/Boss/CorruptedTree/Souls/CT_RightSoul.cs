using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_RightSoul : BaseEnemy
{
    [SerializeField] private CT_RightDummy dummySoulPrefab;
    [SerializeField] private CT_RightDummy dummySoul;

    public override void TriggerAI()
    {
        Debug.Log("RightSoulAI");
        BaseCharacter ally = null;
        ally = BattleUtils.FindAllyFromIndex(0);
        BattleManager.GetInstance.SkillSelected(activeSkills[0]);
        BattleManager.GetInstance.CharacterSelected(ally);
        BattleManager.GetInstance.ExecuteSelectedSkill(ally);
    }


    // Start is called before the first frame update
    void Start()
    {
        CorruptedTree tree = BattleManager.GetInstance.Enemies.formation[1] as CorruptedTree;
        if (tree)
        {
            tree.RightSoul = this;
        }
        Formation enemyformation = BattleManager.GetInstance.Enemies;
        dummySoul = Instantiate(dummySoulPrefab, enemyformation.gameObject.transform).GetComponent<CT_RightDummy>();
        dummySoul.gameObject.SetActive(false);

    }

    public override void SetDead()
    {
        base.SetDead();
        dummySoul.transform.position = this.gameObject.transform.position;
        dummySoul.RowOrder = RowOrder;
        dummySoul.gameObject.SetActive(true);
        BattleManager.GetInstance.Enemies.formation[3] = dummySoul;
    }
}
