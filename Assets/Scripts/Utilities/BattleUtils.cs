using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUtils
{
    /// <summary>
    /// 주어진 Formation에서 가장 HP가 적은 캐릭터를 특정 인덱스 집합 내에서 찾는다.
    /// </summary>
    /// <param name="formationClass">Formation 객체</param>
    /// <param name="indices">탐색할 인덱스의 집합</param>
    /// <returns>가장 HP가 적은 캐릭터, 없으면 null</returns>
    public static BaseCharacter FindCharacterWithLeastHP(Formation formationClass, params int[] indices)
    {
        BaseCharacter characterWithLeastHP = null;
        int lowestHP = int.MaxValue;

        // 주어진 인덱스 집합 내에서 캐릭터를 찾음
        foreach (int index in indices)
        {
            if (index < 0 || index >= formationClass.formation.Length)
                continue; // 유효하지 않은 인덱스는 무시

            BaseCharacter character = formationClass.formation[index];
            if (character != null)
            {
                int currentHP = character.Health.CurHealth;
                if (currentHP < lowestHP)
                {
                    lowestHP = currentHP;
                    characterWithLeastHP = character;
                }
            }
        }

        return characterWithLeastHP;
    }
    
    /// <summary>
    /// 주어진 Formation에서 무작위 캐릭터를 특정 인덱스 집합 내에서 찾는다.
    /// </summary>
    /// <param name="formationClass">Formation 객체</param>
    /// <param name="indices">탐색할 인덱스의 집합</param>
    /// <returns>무작위 캐릭터, 없으면 null</returns>
    public static BaseCharacter FindRandomCharacter(Formation formationClass, params int[] indices)
    {
        List<BaseCharacter> characters = new List<BaseCharacter>();

        // 주어진 인덱스 집합 내에서 캐릭터를 리스트에 추가
        foreach (int index in indices)
        {
            if (index < 0 || index >= formationClass.formation.Length)
                continue; // 유효하지 않은 인덱스는 무시

            BaseCharacter character = formationClass.formation[index];
            if (character != null)
            {
                characters.Add(character);
            }
        }

        if (characters.Count > 0)
        {
            System.Random random = new System.Random();
            int randomIndex = random.Next(0, characters.Count);
            return characters[randomIndex];
        }

        return null; // 집합 내에 캐릭터가 없을 경우
    }
    
    /// <summary>
    /// Allies Formation에서 특정 인덱스 집합 내 가장 HP가 적은 아군을 찾습니다.
    /// </summary>
    /// <param name="indices">탐색할 인덱스의 집합</param>
    /// <returns>가장 HP가 적은 아군, 없으면 null</returns>
    public static BaseCharacter FindAllyWithLeastHP(params int[] indices)
    {
        Formation allies = BattleManager.GetInstance.Allies;
        return FindCharacterWithLeastHP(allies, indices);
    }

    /// <summary>
    /// Allies Formation에서 특정 인덱스 집합 내 무작위 아군을 찾습니다.
    /// </summary>
    /// <param name="indices">탐색할 인덱스의 집합</param>
    /// <returns>무작위 아군, 없으면 null</returns>
    public static BaseCharacter FindRandomAlly(params int[] indices)
    {
        Formation allies = BattleManager.GetInstance.Allies;
        return FindRandomCharacter(allies, indices);
    }

    /// <summary>
    /// Enemies Formation에서 특정 인덱스 집합 내 가장 HP가 적은 적을 찾습니다.
    /// </summary>
    /// <param name="indices">탐색할 인덱스의 집합</param>
    /// <returns>가장 HP가 적은 적, 없으면 null</returns>
    public static BaseCharacter FindEnemyWithLeastHP(params int[] indices)
    {
        Formation enemies = BattleManager.GetInstance.Enemies;
        return FindCharacterWithLeastHP(enemies, indices);
    }

    /// <summary>
    /// Enemies Formation에서 특정 인덱스 집합 내 무작위 적을 찾습니다.
    /// </summary>
    /// <param name="indices">탐색할 인덱스의 집합</param>
    /// <returns>무작위 적, 없으면 null</returns>
    public static BaseCharacter FindRandomEnemy(params int[] indices)
    {
        Formation enemies = BattleManager.GetInstance.Enemies;
        return FindRandomCharacter(enemies, indices);
    }
}
