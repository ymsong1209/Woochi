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
        description = "무작위 부적을 획득합니다";
        return base.GetDescription();
    }
}
