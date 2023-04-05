namespace PfannenkuchenBot;
using System.Text;
using Discord.WebSocket;
abstract class Command
{
    public Command(SocketMessage _socketmsg, string[] _command)
    {
        this.channel = _socketmsg.Channel;
        this.author = _socketmsg.Author;
        this.command = _command;
        this.message = new StringBuilder();
    }
    protected virtual void Send()
    {
        channel.SendMessageAsync(message.ToString());
    }
    protected readonly ISocketMessageChannel channel;
    protected readonly SocketUser author;
    protected readonly string[] command;
    protected StringBuilder message;
}