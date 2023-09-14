namespace PfannenkuchenBot.Commands;
using Discord;
using Discord.WebSocket;
using System.Reflection;

public static class CommandHandler
{
    static readonly Type[] syntaxParameterTypes;
    public static Dictionary<MethodBase, ParameterInfo[]> loadedCommands;
    static CommandHandler()
    {
        Type[] gameElementAssignableTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(GameElement).IsAssignableFrom(type) && type != typeof(GameElement)).ToArray();
        syntaxParameterTypes = gameElementAssignableTypes.Concat(
            new Type[] {
                typeof(ulong),
                typeof(string),
                typeof(bool),
                typeof(Playerdata)
                }).ToArray();
        loadedCommands = LoadCommands();
    }
    static Dictionary<MethodBase, ParameterInfo[]> LoadCommands()
    {
        Dictionary<MethodBase, ParameterInfo[]> loadedCommands = new();
        MethodInfo[] CommandClassMethods = typeof(Command).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        foreach (MethodInfo methodInfo in CommandClassMethods)
        {
            if (Attribute.IsDefined(methodInfo, typeof(CommandAttribute)))
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();
                foreach (ParameterInfo parameterInfo in parameters)
                    if (!syntaxParameterTypes.Contains(parameterInfo.ParameterType)) throw new InvalidSyntaxException(methodInfo.Name);
                loadedCommands.Add(methodInfo, parameters);
            }
        }
        return loadedCommands;
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
        foreach (KeyValuePair<MethodBase, ParameterInfo[]> pair in loadedCommands)
            if (pair.Key.Name.Equals(Format.StripMarkDown(commandMessage[0]).ToString(), StringComparison.OrdinalIgnoreCase))
            {
                Command command = Command.GetCommand(socketMesssage.Author.Username);
                command.currentCommandMessage = commandMessage;
                command.currentSocketMessage = socketMesssage;

                object[] parameters = new object[pair.Value.Length];


                //* This is relevant for parameters, so that the right overload of the method you're trying to invoke can be selected
                //* Also, parsing happens here
                for (int i = 0; i < pair.Value.Length; ++i)
                {
                    if (commandMessage.Length < i + 2) continue;
                    if (pair.Value[i].ParameterType.Equals(typeof(string)))
                    {
                        parameters[i] = commandMessage[i + 1];
                    }
                    else if (pair.Value[i].ParameterType.Equals(typeof(bool)))
                    {
                        if (bool.TryParse(commandMessage[i + 1], out bool boolean)) parameters[i] = boolean;
                    }
                    else if (pair.Value[i].ParameterType.Equals(typeof(ulong)))
                    {
                        if (ulong.TryParse(commandMessage[i + 1], out ulong number)) parameters[i] = number;
                    }
                    else if (pair.Value[i].ParameterType.Equals(typeof(Item)))
                    {
                        if (GameElementLoader.TryGet(commandMessage[i + 1], out Item item)) parameters[i] = item;
                    }
                    else if (pair.Value[i].ParameterType.Equals(typeof(Playerdata)))
                    {
                        parameters[i] = Playerdata.GetPlayerdata(commandMessage[i + 1]); // ! temporary solution, playerdata fetching neads to be reviewed
                    }
                    else throw new Exception("What da heeeeel");
                }

                //* Here it is assured that the cooldown is overa and that it is allowed for the command to be invoked by the user (planned)
                CommandAttribute attribute = (CommandAttribute)pair.Key.GetCustomAttribute(typeof(CommandAttribute))!;
                if (attribute.Cooldown > 0)
                {
                    string timestampName = "lastCalled" + pair.Key.Name;
                    if (!command.player.Timestamps.ContainsKey(timestampName)) command.player.Timestamps.Add(timestampName, DateTime.Now);
                    DateTime lastCalled = command.player.Timestamps[timestampName];
                    TimeSpan cooldown = TimeSpan.FromSeconds(attribute.Cooldown);
                    if (DateTime.Now - lastCalled <= cooldown)
                    {
                        TimeSpan nextAvailableCall = lastCalled + cooldown - DateTime.Now;
                        command.message.Clear();
                        command.message.Append($"You have to wait **{((nextAvailableCall.Hours < 0) ? (nextAvailableCall.Hours + "h ") : "")}{nextAvailableCall.Minutes}min and {nextAvailableCall.Seconds}s**");
                        command.Send();
                        return;
                    }
                }
                try
                {
                    pair.Key.Invoke(command, parameters);
                }
                catch (ArgumentException)
                {
                    command.message.Clear();
                    command.message.Append("Well, seems like your System.Reflection based command invocation was not as robust as you thought, Markus. <@500953493918449674>");
                    command.Send();
                    return;
                }
                if (command.success && attribute.Cooldown > 0)
                {
                    command.player.Timestamps["lastCalled" + pair.Key.Name] = DateTime.Now;
                }
                command.Send();
                return;
            }
        Command.Unknown(socketMesssage.Channel);
    }
}