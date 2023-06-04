namespace PfannenkuchenBot.Commands;
using Discord;
using Discord.WebSocket;
using System.Reflection;
public static class CommandHandler
{
    static MethodBase[] commands;
    static CommandHandler()
    {
        commands = typeof(Command).GetMethods(BindingFlags.DeclaredOnly|BindingFlags.Public|BindingFlags.Instance);
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
        string[] command_message = socketMessage.Content.Remove(0, Config.prefix.Length).Split(' ');
        EvaluateCommand(command_message, socketMessage);
        return Task.CompletedTask;
    }
    static void EvaluateCommand(string[] commandMessage, SocketUserMessage socketmsg)
    {
        foreach (MethodBase methodBase in commands) if (MakeMessageProcessable(commandMessage[0]) == methodBase.Name.ToLower())
            {
                Command command = Command.GetCommand(socketmsg.Author.Id.ToString());
                methodBase.Invoke(command, new object[]{socketmsg, commandMessage});
                return;
            }
        Command.Unknown(socketmsg.Channel);
    }
    public static string MakeMessageProcessable(string msg) => Format.StripMarkDown(msg).ToLower();
}