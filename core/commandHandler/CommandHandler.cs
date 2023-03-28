namespace PfannenkuchenBot.Core;
using Discord;
using Discord.WebSocket;
using System.Reflection;
public static class CommandHandler
{
    static CommandHandler()
    {
        StaticCommands = typeof(Command).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
        InstanceCommands = typeof(Command).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
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
        foreach (MethodBase methodBase in StaticCommands) if (MakeMessageProcessable(command[0]) == methodBase.Name.ToLower())
            {
                methodBase.Invoke(null, new object[] { socketmsg.Channel, command }); return;
            }
        foreach (MethodBase methodBase in InstanceCommands) if (MakeMessageProcessable(command[0]) == methodBase.Name.ToLower())
            {
                methodBase.Invoke(new Command(socketmsg, command), new object[] { }); return;
            }
        Command.Unknown(socketmsg.Channel);
    }
    public static string MakeMessageProcessable(string msg) => Format.StripMarkDown(msg).ToLower();
    public static readonly MethodInfo[] StaticCommands;
    public static readonly MethodInfo[] InstanceCommands;
}