

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
public enum SkillRadius
{
    SingularEnemy,      // 적 단일 지정
    SingularAlly,       // 아군 단일 지정
    MulipleEnemy,       // 적 다수 지정
    MultipleAlly,       // 아군 다수 지정
    END
}

public enum SkillType
{
    Attack,             // 공격 관련
    Heal,               // 힐 관련
    END
}

public enum BuffType
{
    Buff,               // 버프, 디버프 관련
    Cleanse,            // 정화 관련
}

/// <summary>
/// Observer pattern에 사용할 EventType
/// </summary>
public enum EventType
{

}
