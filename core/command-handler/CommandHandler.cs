namespace PfannenkuchenBot.Core;
using Discord;
using Discord.WebSocket;
using System.Reflection;
public static class CommandHandler
{
    static CommandHandler()
    {
        nonInstanceCommands = typeof(NonInstanceCommand).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        instanceCommands = typeof(InstanceCommand).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
    }
    public static Task HandleCommand(SocketMessage _socketMessage)
    {
        var socketMessage = _socketMessage as SocketUserMessage;
        if (socketMessage == null) return Task.CompletedTask;
        if (!(
        socketMessage.Content.StartsWith(Config.prefix)) ||
        socketMessage.Author.IsBot ||
        socketMessage.Content.Length <= Config.prefix.Length
        ) return Task.CompletedTask;
        string[] command = socketMessage.Content.Remove(0, Config.prefix.Length).Split(' ');
        EvaluateCommand(command, socketMessage);
        return Task.CompletedTask;
    }
    static void EvaluateCommand(string[] command, SocketUserMessage socketmsg)
    {
        foreach (MethodBase methodBase in nonInstanceCommands) if (MakeMessageProcessable(command[0]) == methodBase.Name.ToLower())
            {
                methodBase.Invoke(new NonInstanceCommand(socketmsg, command), null); return;
            }
        foreach (MethodBase methodBase in instanceCommands) if (MakeMessageProcessable(command[0]) == methodBase.Name.ToLower())
            {
                methodBase.Invoke(new InstanceCommand(socketmsg, command),null); return;
            }
        Command.Unknown(socketmsg.Channel);
    }
    public static string MakeMessageProcessable(string msg) => Format.StripMarkDown(msg).ToLower();
    public static readonly MethodInfo[] nonInstanceCommands;
    public static readonly MethodInfo[] instanceCommands;
}