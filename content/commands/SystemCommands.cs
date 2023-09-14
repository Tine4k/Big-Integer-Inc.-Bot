using System.Reflection;

namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Help()
    {
        message.Append("**List of all available commands:**\n");
        foreach (MethodBase command in CommandHandler.loadedCommands.Keys.Where(method => !method.IsVirtual).ToArray()) 
        message.Append(Config.prefix + command.Name.ToLower() + '\n');

    }
}