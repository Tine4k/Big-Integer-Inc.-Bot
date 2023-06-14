using System.Reflection;
using Discord.WebSocket;

namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Help()
    {
        message.Append("**List of all available commands:\n**");
        foreach (MethodBase command in CommandHandler.loadedCommands) message.Append(Config.prefix + command.Name.ToLower() + '\n');
        
    }
}