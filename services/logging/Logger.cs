using Discord;
using PfannenkuchenBot.Commands;
using PfannenkuchenBot.ConsolePort;
namespace PfannenkuchenBot;
public class Logger
{
    public static readonly string logDirectory = @"log\";

    public static string currentLogPath = null!;

    // static StreamWriter writer = null!;

    // static void WriteToLogFile(LogMessage logMessage) => writer.WriteLine(logMessage.ToString());

    public static Task Log(LogMessage logMessage)
    {
        currentLogPath ??= $"{logDirectory}{Program.SessionId}.log"; ;
        // writer ??= new StreamWriter(currentLogPath, true);
        if (DateTime.Now - ConsolePorter.lastUsed > TimeSpan.FromSeconds(8)) Console.WriteLine(logMessage);
        using StreamWriter writer = new(currentLogPath, true); writer.WriteLine(logMessage); //! Temporary solution, make a class member at some point and add hybernate function to bot
        // WriteToLogFile(logMessage);
        return Task.CompletedTask;
    }

    public static Task Log(CommandHandler handler)
    {
        handler.message.Insert(0, $"{handler.player.Username} issued \"{string.Join(' ', handler.currentCommandMessage)}\":\n");
        return Log(handler.message.ToString(), source: "Commands");
    }

    public static Task Log(string message, string source = "Unspecified", LogSeverity severity = LogSeverity.Info)
    {
        LogMessage logMessage = new(severity, source, message.Replace('\n', ' '));
        return Log(logMessage);
    }
}