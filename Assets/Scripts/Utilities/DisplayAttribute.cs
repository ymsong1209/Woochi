using System;

[AttributeUsage(AttributeTargets.Field)]
public class DisplayAttribute : Attribute
{
    public string DisplayName { get; private set; }

    public DisplayAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}
