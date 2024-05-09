

public enum GameState
{
    MAINMENU,
    SELECTROOM,
    BATTLE,
    END

}

public enum BattleState
{
    IDLE,               // ������
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
public enum SkillTargetType
{
    Singular,   //Singular�� ��� skillRadius�� üũ�� ������ �ϳ��� ���� ����
    Multiple,   //Multiple�� ��� skillRadius�� üũ�� ������ŭ ��ü ��ų ����
    END
}

public enum SkillType
{
    Attack,             // ���� ����
    Heal,               // �� ����
    END
}

public enum BuffType
{
    Bleed,
    Weaken,
    Protect,
    Shield,      
    Stun,
    StatChange,
    Burn,
    SpiritAbsorption,
    END
}

public enum BuffTiming
{
    BattleStart,
    RoundStart,
    RoundEnd,
    TurnStart,
    BattleEnd,
    END
}


/// <summary>
/// Observer pattern�� ����� EventType
/// </summary>
public enum EventType
{

}

/// <summary>
/// � �ִϸ��̼��� �������� ����, ��ų �� ���� ��� �߰�
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
    END
}

/// <summary>
/// ĳ������ ���� ���
/// </summary>
public enum AttackResult
{
    Miss,
    Evasion,
    Normal,
    END
}
