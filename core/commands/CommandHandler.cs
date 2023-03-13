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
        if ( !(
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
        foreach (MethodBase methodBase in StaticCommands) if (ProcessablifyMessage(command[0]) == methodBase.Name.ToLower()) { methodBase.Invoke(null, new object[] { socketmsg.Channel }); return; }
        foreach (MethodBase methodBase in InstanceCommands) if (ProcessablifyMessage(command[0]) == methodBase.Name.ToLower()) { methodBase.Invoke(new Command(socketmsg), new object[] { }); return; } // debug m.Invoke(typeof(Command).GetConstructor(new Type[]{typeof(SocketMessage)}).Invoke(new Object[]{socketmsg}), new object[] { }); return;
        Command.Unknown(socketmsg.Channel);
    }
    public static string ProcessablifyMessage(string msg) => Format.StripMarkDown(msg).ToLower();
    static readonly MethodInfo[] StaticCommands;
    static readonly MethodInfo[] InstanceCommands;
}