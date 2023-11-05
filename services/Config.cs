using System.Reflection;
using System.ComponentModel;
static public class Config
{
    static readonly FieldInfo[] configFields;
    static Config()
    {
        configFields = typeof(Config).GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public);
        Reload();
    }
    public static void Reload()
    {
        foreach (FieldInfo field in configFields) ApplyConfig(field);
    }
    static void ApplyConfig(FieldInfo field)
    {
        string? configValue = GetConfigValue(field.Name);
        if (string.IsNullOrWhiteSpace(configValue)) return;
        if (field.FieldType == typeof(string)) field.SetValue(null, configValue);
        else
        {
            object? convertedValue = SetFieldValue(field, configValue);
            field.SetValue(null, convertedValue);
        }
        
        object? SetFieldValue(FieldInfo fieldInfo, string configValue)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(fieldInfo.FieldType);
            object? convertedValue = Convert.ChangeType(converter.ConvertFromString(configValue), fieldInfo.FieldType);
            return convertedValue;
        }
    }
    static string GetConfigValue(string fieldName)
    {
        if (File.Exists("config/"+fieldName)) return File.ReadAllText("config/"+fieldName);
        else return string.Empty;
    }

    public static string prefix = "*";
    public static bool autoUnload = true;
    public static bool forceUnload = false;
    public static ushort autoUnloadInterval = 60;
    public static ushort idleUnloadTime = 60;
    public static char currency = '$';
    public static bool logAllCommands = true;
    public static bool logPlayerdataLoads = true;
    public static bool logPlayerdataUnloads = true;
    public static bool logPlayerdataCreations = true;
    
}