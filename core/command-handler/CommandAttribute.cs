using PfannenkuchenBot.Commands;

[AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
sealed class CommandAttribute : Attribute
{

    // This is a positional argument
    public CommandAttribute(Type[] Syntax, CommandCategory Category)
    {
        foreach (Type type in Syntax)
        {
            if (!CommandHandler.syntaxParameterTypes.Contains(type)) throw new InvalidSyntaxException();
        }
        this.Syntax = Syntax;
        this.Category = Category;
    }
    public Type[] Syntax { get; }
    public CommandCategory Category { get; }
    public TimeSpan Cooldown { get; set; }
}