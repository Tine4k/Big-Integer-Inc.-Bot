using System.Reflection;
using System.ComponentModel;
static class Config
{
    static FieldInfo[] configFields;
    static Config()
    {
        configFields = typeof(Config).GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public);
        ReloadConfig();
    }
    public static void ReloadConfig()
    {
        foreach (FieldInfo fieldInfo in configFields)
        {
            ApplyConfigToField(fieldInfo);
        }
    }
    static void ApplyConfigToField(FieldInfo fieldInfo)
    {
        string? configValue = GetConfigValue(fieldInfo.Name);
        if (String.IsNullOrWhiteSpace(configValue)) return;
        if (fieldInfo.FieldType == typeof(string)) fieldInfo.SetValue(null, configValue);
        else
        {
            object? convertedValue = CastFieldValueToFieldType(fieldInfo, configValue);
            fieldInfo.SetValue(null, convertedValue);
        }
        return;
        object? CastFieldValueToFieldType(FieldInfo fieldInfo, string configValue)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(fieldInfo.FieldType);
            object? convertedValue = Convert.ChangeType(converter.ConvertFromString(configValue), fieldInfo.FieldType);
            return convertedValue;
        }
    }
    static string? GetConfigValue(string fieldName)
    {
        return String.Empty;
    }

    public static string prefix = "*";
    public static bool autoUnload = true;
    public static bool forceUnload = false;
    public static ushort autoUnloadInterval = 60;
    public static ushort idleUnloadTime = 60;
}