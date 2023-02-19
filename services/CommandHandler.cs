namespace PfannenkuchenBot;
using Discord;
using Discord.WebSocket;
using System.Reflection;
static public class CommandHandler
{
    static CommandHandler()
    {
        StaticCommands = typeof(Command).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
        InstanceCommands = typeof(Command).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
    }
    public static Task HandleCommand(SocketMessage msg)
    {
        var socketmsg = msg as SocketUserMessage;
        if (socketmsg == null) return Task.CompletedTask;
        if (!(socketmsg.Content.StartsWith(Config.prefix)) || socketmsg.Author.IsBot || socketmsg.Content.Length == 1) return Task.CompletedTask;
        string[] cmd = socketmsg.Content.Remove(0, Config.prefix.Length).Split(' ');
        EvaluateCommand(cmd, socketmsg);
        return Task.CompletedTask;
    }
    static void EvaluateCommand(string[] cmd, SocketUserMessage socketmsg)
    {
        foreach (MethodBase m in StaticCommands) if (ConvMsgToCmd(cmd[0]) == m.Name.ToLower()) { m.Invoke(null, new object[] { socketmsg.Channel }); return; }
        foreach (MethodBase m in InstanceCommands) if (ConvMsgToCmd(cmd[0]) == m.Name.ToLower()) { m.Invoke(new Command(socketmsg), new object[] { }); return; } // debug m.Invoke(typeof(Command).GetConstructor(new Type[]{typeof(SocketMessage)}).Invoke(new Object[]{socketmsg}), new object[] { }); return;
        Command.Unknown(socketmsg.Channel);
    }
    public static string ConvMsgToCmd(string msg) => Format.StripMarkDown(msg).ToLower();
    static readonly MethodInfo[] StaticCommands;
    static readonly MethodInfo[] InstanceCommands;
}