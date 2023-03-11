using Discord.WebSocket;
using PfannenkuchenBot.Core;
namespace PfannenkuchenBot;
partial class Command
{
    public Command(SocketMessage _socketmsg)
    {
        this.channel = _socketmsg.Channel;
        this.author = _socketmsg.Author;
        this.instanceHandler = InstanceHandler.GetInstanceHandler(_socketmsg.Author.Id.ToString());
        this.playerdata = instanceHandler.playerdata;
    }
    readonly Playerdata playerdata;
    readonly InstanceHandler instanceHandler;
    readonly ISocketMessageChannel channel;
    readonly SocketUser author;
}