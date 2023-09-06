﻿namespace PfannenkuchenBot;
using PfannenkuchenBot.Commands;
using Discord;
using Discord.WebSocket;
using System.Text.Json.Serialization;
using System.Text.Json;

static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            if (args[0] == "test")
            {
                Console.WriteLine("Test running...");

                Log(JsonSerializer.Serialize(new string[]{"Illegal", "Unstackable"}), "Startup");
            }
        }
        else 
        StartUp().GetAwaiter().GetResult();
    }
    public static DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig()
    {
        GatewayIntents =
        GatewayIntents.Guilds |
        GatewayIntents.GuildEmojis |
        GatewayIntents.GuildMessages |
        GatewayIntents.GuildMessageReactions |
        GatewayIntents.MessageContent
    });
    static async Task StartUp()
    {
        client.Log += Log;
        Program.client.MessageReceived += CommandHandler.HandleCommand;
        if (!File.Exists("config/token.txt")) throw new FileNotFoundException("File \"token.txt\" not found. Please provice a text file with a valid token.");
        await client.LoginAsync(TokenType.Bot, File.ReadAllText("config/token.txt"));
        await client.StartAsync();
        await Task.Delay(-1);
    }
    static Task Log(LogMessage logMessage)
    {
        Console.WriteLine(logMessage);
        return Task.CompletedTask;
    }
    public static Task Log(string message, string source = "Unspecified", LogSeverity severity = LogSeverity.Info)
    {
        LogMessage logMessage = new(severity,source,message.Replace('\n', ' '));
        return Log(logMessage);
    }
}