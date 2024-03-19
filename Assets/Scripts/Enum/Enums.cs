

public enum GameState
{
    MAINMENU,
    SELECTROOM,
    BATTLE,
    END

}

public enum BattleState
{
    Initialization,     // ���� ���� ������ ������
    Prebattle,          // ���� ���� �� ĳ���͵��� ��ġ�� ����, ���� ó��
    DetermineOrder,     // ĳ���͵��� �ӵ����� ���缭 ���� ����
    CharacterTurn,      // �ӵ����� ���� ĳ���͵� �ൿ
    PostTurn,           // �� ���尡 ���� ���� ���� ȿ�� ó��
    VictoryCheck,       // �¸� üũ, ������ ���������� Prebattle�� �Ѿ
    PostBattle,         // ������ ���� �� ���� ���� ����
    END
}