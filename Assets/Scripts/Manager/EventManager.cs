using System;

public class EventManager : SingletonMonobehaviour<EventManager>
{
    public Action onChangedGold;        // 골드 변경 시 호출되는 함수
    public Action<bool> onSelectReward;       // 보상 선택 시 호출되는 함수
    public Action<int, bool> onCanUseTurn;
}
