using System;

public class EventManager : SingletonMonobehaviour<EventManager>
{
    public Action onChangedGold;        // ��ȭ ��ȭ �� ȣ���� �Լ���
    public Action<bool> onSelectReward;       // ���� ���� �� ȣ���� �Լ�
}
