namespace PfannenkuchenBot.Commands;
using Discord;
using Discord.WebSocket;
using System.Reflection;
public static class CommandHandler
{
    public static readonly Type[] syntaxParameterTypes;
    public static MethodBase[] loadedCommands;
    static CommandHandler()
    {
        syntaxParameterTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(GameElement).IsAssignableFrom(type) && type != typeof(GameElement)).
            Concat(new Type[] { typeof(long), typeof(string) }).ToArray();
        loadedCommands = LoadCommands();
    }
    static MethodInfo[] LoadCommands()
    {
        List<MethodInfo> loadedCommands = new(); 
        MethodInfo[] CommandMethods = typeof(Command).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        foreach (MethodInfo methodInfo in CommandMethods)
        {
            if (Attribute.IsDefined(methodInfo, typeof(CommandAttribute)))
            {
                loadedCommands.Add(methodInfo);
                CommandAttribute cmd = (CommandAttribute)methodInfo.GetCustomAttribute(typeof(Command))!;
                cmd.TargetedMethod = methodInfo;
                cmd.Syntax = methodInfo.GetParameters();
                foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
                    if (!syntaxParameterTypes.Contains(parameterInfo.ParameterType)) throw new InvalidSyntaxException(methodInfo.Name);
            }
        }
        return loadedCommands.ToArray();
    }
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
                CommandAttribute commandAttribute = (CommandAttribute)methodBase.GetCustomAttribute(typeof(CommandAttribute))!;
                command.currentCommandMessage = commandMessage;
                command.currentSocketMessage = socketMesssage;

                object[] parameters;

                for (int i = 1; i < commandAttribute.Syntax.Length; ++i)
                {
                    // TODO Parsing
                }

                methodBase.Invoke(command, parameters);
                command.Send();
                return;
            }
        Command.Unknown(socketMesssage.Channel);
    }
}