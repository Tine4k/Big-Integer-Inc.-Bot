using System.Reflection;

namespace PfannenkuchenBot.Commands;
public partial class CommandHandler
{
    [Command(CommandCategory.System)]
    public void Help()
    {
        message.AppendLine("**List of all available commands:**");
        foreach (MethodBase command in CommandHandler.LoadedCommands.Keys.Where(method => !method.IsVirtual).ToArray()) 
        message.AppendLine(Config.prefix + command.Name.ToLower());

    }
}