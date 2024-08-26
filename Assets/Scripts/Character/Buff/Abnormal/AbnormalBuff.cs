using Unity.VisualScripting;

public class AbnormalBuff : StatDeBuff
{
    public bool applyAlly = true;
    public bool applyEnemy = false;

    public AbnormalBuff()
    {
        isRemovableDuringBattle = false;
    }
}
