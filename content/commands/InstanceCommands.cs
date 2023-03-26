using Discord.WebSocket;
using PfannenkuchenBot.Core;
namespace PfannenkuchenBot;
partial class Command
{
    // * All commands that reference playerdata belong here
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
    public void Daily()
    {
        playerdata.Gain(1000);
        channel.SendMessageAsync($"Added 1000$ to your balance, which now contains {playerdata.Balance}$");
    }
    public void Balance()
    {
        channel.SendMessageAsync($"Your current balance is {playerdata.Balance}$");
    }
}