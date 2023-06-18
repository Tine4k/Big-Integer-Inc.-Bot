namespace PfannenkuchenBot.Commands;
using Discord;
using Discord.WebSocket;
using System.Reflection;
public static class CommandHandler
{
    public static MethodBase[] loadedCommands;
    static CommandHandler()
    {
        loadedCommands = LoadCommands();
    }
    static MethodInfo[] LoadCommands() => typeof(Command).GetMethods(BindingFlags.DeclaredOnly|BindingFlags.Public|BindingFlags.Instance); 
    public static Task HandleCommand(SocketMessage _socketMessage)
    {
        var socketMessage = _socketMessage as SocketUserMessage;
        if (socketMessage is null) return Task.CompletedTask;
        if (!(
        socketMessage.Content.StartsWith(Config.prefix)) ||
        socketMessage.Author.IsBot ||
        socketMessage.Content.Length <= Config.prefix.Length
        ) return Task.CompletedTask;
        string[] command_message = socketMessage.Content.Remove(0, Config.prefix.Length).Split(' ');
        EvaluateCommand(command_message, socketMessage);
        return Task.CompletedTask;
    }
    static void EvaluateCommand(string[] commandMessage, SocketUserMessage socketMesssage)
    {
        foreach (MethodBase methodBase in loadedCommands) 
            if (methodBase.Name.Equals(Format.StripMarkDown(commandMessage[0]).ToString(), StringComparison.OrdinalIgnoreCase))
            {
                Command command = Command.GetCommand(socketMesssage.Author.Username);
                command.currentCommandMessage = commandMessage;
                command.currentScketMessage = socketMesssage;
                methodBase.Invoke(command, null);
                command.Send();
                return;
            }
        Command.Unknown(socketMesssage.Channel);
    }
}