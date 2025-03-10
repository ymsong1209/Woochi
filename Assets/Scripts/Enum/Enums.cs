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
    SingularWithoutSelf, //스킬 범위 내에서 자기 자신 제외하고 고를 수 있음.
    Self,       //자기 자신만 선택 가능
    Random, //스킬 범위 내에서 랜덤으로 선택
    END
}

public enum SkillType
{
    Attack,             // 공격 관련
    Heal,               // 힐 관련
    SpecialNegative,    // 특수기, 상대 피격 애니메이션
    SpecialPositive,    // 특수기, 상대 힐 애니메이션
    CustomSkill,        // 애니메이션 재정의 필요
    END
}

public enum BuffStackType
{
    Default,
    ResetDuration,          //갱신 : 지속시간 처음으로 초기화
    StackEffect,            //중첩 : 효과 중첩하여 적용
    ExtendDuration,         //연장 : 남은 시간 + 새 지속시간
    ResetAndStack,          //갱신 및 중첩 : 지속시간 처음으로 초기화 + 효과 중첩
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
    Heal,
    Stun,
    END
}

public enum CharmTargetType
{
    Singular,
    Multiple,
    SingularWithSelf,
    //MultipleWithSelf,
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
    IncreaseCritOvertime,       //족자구 회피 증가 버프
    Troll,
    Poison,
    Fear,                       //최종 대미지 감소
    StunResist,
    MoveResist,                 //강제이동 저항
    Special,
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
    PostHit,
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
    Tutorial,
    END
}

public enum RareType
{
    Lowest,
    Low,
    Middle,
    High,
    Highest,
    END
}

public enum StatType
{
    [Display("체력")] Health,
    [Display("속도")] Speed,
    [Display("방어")] Defense,
    [Display("치명")] Crit,
    [Display("명중")] Accuracy,
    [Display("회피")] Evasion,
    [Display("저항")] Resist,
    [Display("피해")] MinDamage,
    [Display("피해")] MaxDamage,
    END
}

public enum StrangeType
{
    Lucky,
    UnKnown,
    UnLucky,
    END
}

public enum UIEvent
{
    MouseEnter,
    MouseExit,
    MouseClick,
    END
}

public enum ScenarioType
{
    Dialogue,
    Narration,
    Guide,
    END
}

public enum PlotEvent
{
    None,       // 이벤트 없음
    Click,      // 클릭 이벤트
    Action,     // 행동 이벤트
    BattleEnd,  // 전투 종료 이벤트
    END
}