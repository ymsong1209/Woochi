using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevel_", menuName = "Scriptable Objects/Dungeon/Dungeon Level")]
public class DungeonInfoSO : ScriptableObject
{
    #region Header MONSTERINFO
    [Space(10)]
    [Header("MONSTER INFO FOR LEVEL")]
    #endregion Header MONSTERINFO

    public List<BaseCharacter> Enemy = new List<BaseCharacter>();

}
