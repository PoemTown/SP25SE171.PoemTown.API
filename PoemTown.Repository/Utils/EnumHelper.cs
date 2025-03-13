using System.ComponentModel;
using System.Reflection;

namespace PoemTown.Repository.Utils;

public static class EnumHelper
{
    public static string GetDescription(Enum? value)
    {
        if(value == null)
        {
            return string.Empty;
        }
        FieldInfo field = value.GetType().GetField(value.ToString());
        DescriptionAttribute attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
}