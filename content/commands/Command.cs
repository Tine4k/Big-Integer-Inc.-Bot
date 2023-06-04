namespace PfannenkuchenBot;
using System.Text;
using Discord.WebSocket;
using Discord;
using PfannenkuchenBot.Core;

abstract class Command
{
    public Command(SocketMessage _socketmsg, string[] _command)
    {
        this.channel = _socketmsg.Channel;
        this.author = _socketmsg.Author;
        this.command = _command;
        this.message = new StringBuilder();
    }
    public static void Unknown(ISocketMessageChannel channel) => 
    channel.SendMessageAsync(Format.BlockQuote($"Unknown Command, please use {Config.prefix}help for a list of available commands!"));
    protected virtual void Unknown() => Unknown(channel);
    protected virtual void Send()
    {
        channel.SendMessageAsync(message.ToString());
        message.Insert(0, $"{author.Username} issued \"{String.Join(' ', command)}\"\n");
        if (Config.logAllCommands) Program.Log(message.ToString());
    }
    readonly ISocketMessageChannel channel;
    protected readonly SocketUser author;
    protected readonly string[] command;
    protected readonly StringBuilder message;
}
