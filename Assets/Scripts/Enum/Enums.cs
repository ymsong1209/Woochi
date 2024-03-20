

public enum GameState
{
    MAINMENU,
    SELECTROOM,
    BATTLE,
    END

}

public enum BattleState
{
    Initialization,     // ���� ���� ������ ������, ĳ���͵��� ��ġ�� ����
    PreRound,           // ���� ���� �� ĳ���� ���� ó��
    DetermineOrder,     // ĳ���͵��� �ӵ����� ���缭 ���� ����
    CharacterTurn,      // �ӵ����� ���� ĳ���͵� �ൿ
    PostRound,          // �� ���尡 ���� ���� ���� ȿ�� ó��
    PostBattle,         // ������ ���� �� ���� ���� ����
    END
}

/// <summary>
/// ��ų�� ��� ����
/// </summary>
public enum SkillRadius
{
    SingularEnemy,      // �� ���� ����
    SingularAlly,       // �Ʊ� ���� ����
    MulipleEnemy,       // �� �ټ� ����
    MultipleAlly,       // �Ʊ� �ټ� ����
    END
}

public enum SkillType
{
    Attack,             // ���� ����
    Heal,               // �� ����
    Buff,               // ����, ����� ����
    END
}

/// <summary>
/// Observer pattern�� ����� EventType
/// </summary>
public enum EventType
{

}
