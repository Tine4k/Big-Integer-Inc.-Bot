using System.Reflection;
using Discord.WebSocket;
using Pfannenkuchenbot.Item;

namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Help(SocketMessage socketmsg, string[] commandMessage)
    {
        message.Append("**List of all available commands:\n**");
        foreach (MethodBase command in CommandHandler.commands) message.Append(Config.prefix + command.Name + "\n");
        Send(message, socketmsg, commandMessage);
    }
}