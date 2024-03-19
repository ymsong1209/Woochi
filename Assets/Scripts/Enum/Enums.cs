

public enum GameState
{
    MAINMENU,
    SELECTROOM,
    BATTLE,
    END

}

public enum BattleState
{
    Initialization,     // 전투 방의 정보를 가져옴
    Prebattle,          // 전투 시작 전 캐릭터들의 위치값 설정, 버프 처리
    DetermineOrder,     // 캐릭터들의 속도값에 맞춰서 순서 결정
    CharacterTurn,      // 속도값에 따라서 캐릭터들 행동
    PostTurn,           // 한 라운드가 끝난 뒤의 버프 효과 처리
    VictoryCheck,       // 승리 체크, 전투가 남아있을시 Prebattle로 넘어감
    PostBattle,         // 전투가 끝난 뒤 전투 보상 정산
    END
}