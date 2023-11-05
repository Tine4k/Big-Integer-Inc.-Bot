using System.Reflection;

namespace PfannenkuchenBot.Commands;
public partial class CommandHandler
{
    [Command(CommandCategory.System)]
    public void Help()
    {
        message.Append("**List of all available commands:**\n");
        foreach (MethodBase command in CommandHandler.LoadedCommands.Keys.Where(method => !method.IsVirtual).ToArray()) 
        message.Append(Config.prefix + command.Name.ToLower() + '\n');

    }
}