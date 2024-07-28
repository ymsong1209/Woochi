using System;

public class EventManager : SingletonMonobehaviour<EventManager>
{
    public Action onChangedGold;        // 재화 변화 시 호출할 함수들
    public Action<bool> onSelectReward;       // 보상 선택 시 호출할 함수
}
