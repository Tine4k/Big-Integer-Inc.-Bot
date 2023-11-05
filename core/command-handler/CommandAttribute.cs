namespace PfannenkuchenBot.Commands;
[AttributeUsage(System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
sealed public class CommandAttribute : Attribute
{

    public CommandAttribute(CommandCategory Category)
    {
        this.Categories = new CommandCategory[] { Category };
    }
    public CommandAttribute(CommandCategory[] Categories)
    {
        this.Categories = Categories;
    }
    public CommandCategory Category { get => Categories[0]; }
    public CommandCategory[] Categories { get; }
    /* The categories this command is relevant to, categories can be deactivated (planned),
    so if you want to deactivate a category, and a command is within two catgories, 
    it still stays active as long as the other category is active*/
    public ulong Cooldown { get; set; }
    /* Cooldown between usage of Command, in seconds*/
}