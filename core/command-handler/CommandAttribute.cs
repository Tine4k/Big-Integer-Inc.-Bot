using System.Reflection;
using PfannenkuchenBot.Commands;

[AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
sealed class CommandAttribute : Attribute
{

    public CommandAttribute(CommandCategory Category)
    {
        this.Category = Category;
        this.TargetedMethod = null!;
        this.Syntax = null!;
    }
    public CommandCategory Category { get; }
    public TimeSpan Cooldown { get; set; }
    public MethodInfo TargetedMethod { private get; set; }
    public ParameterInfo[] Syntax { get; set; }
}