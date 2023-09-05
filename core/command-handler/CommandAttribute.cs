using System.Reflection;
using PfannenkuchenBot.Commands;

[AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
sealed class CommandAttribute : Attribute
{

    public CommandAttribute(CommandCategory Category, TimeSpan Cooldown)
    {
        this.Category = Category;
        this.Cooldown = Cooldown;
    }
    public CommandCategory Category { get; }
    public TimeSpan Cooldown { get; }
    public ParameterInfo[]? Syntax {get; set;}
}