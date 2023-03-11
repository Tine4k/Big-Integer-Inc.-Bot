using System.Reflection;
namespace PfannenkuchenBot;
static class Config
{
    static Config()
    {
        foreach (FieldInfo fieldInfo in typeof(Config).GetFields(BindingFlags.DeclaredOnly))
        {
            if (File.Exists($"config/{fieldInfo.Name}.json")) fieldInfo.SetValue(null, File.ReadAllText($"config/{fieldInfo.Name}.json"));
        }
    }
    public static readonly string prefix = "*";
    public static readonly ushort autoUnloadInterval = 60;
    public static readonly ushort idleUnloadTime = 60;
    public static readonly bool autoUnload = true;
    public static readonly bool forceUnload = true;
}