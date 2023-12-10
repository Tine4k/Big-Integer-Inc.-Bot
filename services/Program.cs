namespace PfannenkuchenBot;
using System;
using DiscordPort;
using WebPort;

public static class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length > 0)
        {
            if (args[0] == "test")
            {
            }
        }
        else await StartUp();
    }

    static Program()
    {
        SessionId = CreateSessionId();
    }

    static string CreateSessionId()
    {
        string today = DateTime.Today.ToString("y-M-d");
        if (Directory.Exists(Logger.logDirectory))
        {
            string[] logsOfToday = Directory.GetFiles(Logger.logDirectory).Where((string a) => Path.GetFileName(a).StartsWith(today)).OrderBy((string a) => a).ToArray();
            return $"{today}-{logsOfToday.Length + 1}";
        }
        return $"{today}-1";
    }

    static async Task StartUp()
    {
        CreateSessionId();
        if (Config.discordPortActive)
        {
            DiscordPorter.StartUp();
            await Logger.Log("Discord started");
        }
        if (Config.webPortActive)
        {
            WebPorter.StartUp();
            await Logger.Log("Webport started");
        }
        await Task.Delay(-1);
    }

    public static readonly string SessionId;
}