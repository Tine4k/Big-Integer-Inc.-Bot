namespace PfannenkuchenBot.Commands;
using Discord;
using Discord.WebSocket;
using System.Reflection;
public static class CommandHandler
{
    static List<MethodBase> commands;
    static CommandHandler()
    {
        Type baseType = typeof(Command);

        IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes()
        .Where(type => baseType.IsAssignableFrom(type) && type != baseType);

        commands = new List<MethodBase>(); 
    }
    public static Task HandleCommand(SocketMessage _socketMessage)
    {
        if (InstanceHandler.loadedInstanceHandlers.TryGetValue(_socketMessage.Author.Id.ToString(), out InstanceHandler? instanceHandler))
        {
            if (instanceHandler is null) throw new NullReferenceException();
            throw new NotImplementedException();
        }
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
        foreach (MethodBase methodBase in commands) if (MakeMessageProcessable(command[0]) == methodBase.Name.ToLower())
            {
                CreateInstanceHandler();
            }
        Command.Unknown(socketmsg.Channel);
        
        Command CreateInstanceHandler()
        {
            throw new NotImplementedException();           
        }
    }
    public static string MakeMessageProcessable(string msg) => Format.StripMarkDown(msg).ToLower();
}