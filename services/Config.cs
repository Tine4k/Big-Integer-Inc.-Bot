using System.Configuration;
using System.Reflection;
using System.ComponentModel;
static class Config
{
    public static void ReloadConfig()
    {
        foreach (FieldInfo fieldInfo in typeof(Config).GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public))
        {
            string? configValue = ConfigurationManager.AppSettings[fieldInfo.Name];
            if (String.IsNullOrWhiteSpace(configValue)) return;
            if (fieldInfo.FieldType == typeof(string)) fieldInfo.SetValue(null, configValue);
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(fieldInfo.FieldType);
                object? convertedValue = Convert.ChangeType(converter.ConvertFromString(configValue), fieldInfo.FieldType);
                fieldInfo.SetValue(null, convertedValue);
            }            
        }
    }
    public static string prefix = "*";
    public static bool autoUnload = true;
    public static bool forceUnload = false;
    public static ushort autoUnloadInterval = 60;
    public static ushort idleUnloadTime = 60;
}