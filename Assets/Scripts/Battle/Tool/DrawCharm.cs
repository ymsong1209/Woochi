public class DrawCharm : Tool
{
    public override void Use()
    {
        if(!HelperUtilities.CanGetCharm(out resultTxt))
        {
            return;
        }

        base.Use();
    }

    public override string GetDescription()
    {
        description = "������ ������ ȹ���մϴ�";
        return base.GetDescription();
    }
}
