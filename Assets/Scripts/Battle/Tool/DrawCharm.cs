public class DrawCharm : Tool
{
    public override void Use()
    {
        if(!HelperUtilities.CanGetCharm())
        {
            return;
        }

        base.Use();
    }

    protected override string GetDescription()
    {
        description = "������ ������ ȹ���մϴ�";
        return base.GetDescription();
    }
}
