public enum GameState
{
    MAINMENU,
    SELECTROOM,
    BATTLE,
    END

}

public enum BattleState
{
    IDLE,               // 비전투
    Initialization,     // 전투 방의 정보를 가져옴, 캐릭터들의 위치값 설정
    PreRound,           // 전투 시작 전 캐릭터 버프 처리
    DetermineOrder,     // 캐릭터들의 속도값에 맞춰서 순서 결정
    CharacterTurn,      // 속도값에 따라서 캐릭터들 행동
    PostRound,          // 한 라운드가 끝난 뒤의 버프 효과 처리
    PostBattle,         // 전투가 끝난 뒤 전투 보상 정산
    END
}

/// <summary>
/// 스킬의 사용 범위
/// </summary>
public enum SkillTargetType
{
    Singular,   //Singular일 경우 skillRadius에 체크된 범위중 하나만 선택 가능
    Multiple,   //Multiple일 경우 skillRadius에 체크된 범위만큼 전체 스킬 적용
    END
}

public enum SkillType
{
    Attack,             // 공격 관련
    Heal,               // 힐 관련
    Special,            // 특수기 대미지 표시가 안뜸.
    END
}
public enum SkillElement
{
    Defualt,
    Fire,
    Water,
    Wood,
    Metal,
    Earth,
    END
}

public enum CharmType
{
    Buff,
    DeBuff,
    CleanseSingleDebuff,
    END
}

public enum CharmTargetType
{
    Singular,
    Multiple,
    SingularWithSelf,
    MultipleWithSelf,
    End
}



public enum BuffEffect
{
    Bleed,
    Burn,
    Stun,
    StatWeaken,
    StatStrengthen,
    DotCure,                    //도트힐
    Protect,                    //보호를 해줄때
    Shield,                     //보호받을때
    IncreaseEvasionOvertime,    //족자구 회피 증가 버프
    DotCureByDamage,            //피해량 기반으로 도트힐
    ElementalStatStrengthen,    //속성별 스탯 강화
    ElementalStatWeaken,
    END
}

public enum BuffType
{
    Default,
    Positive, //긍정적인 버프
    Negative, //부정적인 버프(디버프)
    END
}

public enum BuffTiming
{
    BattleStart,
    RoundStart,
    RoundEnd,
    TurnStart,
    TurnEnd,
    BattleEnd,
    END
}

/// <summary>
/// 어떤 애니메이션을 실행할지 결정, 스킬 더 있을 경우 추가
/// </summary>
public enum AnimationType
{
    Idle,
    Damaged,
    Dead,
    Skill0,
    Skill1,
    Skill2,
    Skill3,
    Skill4,
    Heal,
    END
}

/// <summary>
/// 캐릭터의 공격 결과
/// </summary>
public enum AttackResult
{
    Miss,
    Evasion,
    Normal,
    Heal,
    END
}

#region 맵
public enum NodeType
{
    Normal,     // 일반
    Elite,      // 정예
    Strange,    // 기연
    Boss,       // 보스
    END
}

public enum NodeState
{
    Locked,
    Visited,
    Attainable
}

#endregion

public enum SceneType
{
    Title,
    Battle,
    Loading,
    END
}