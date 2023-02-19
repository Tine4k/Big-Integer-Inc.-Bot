namespace PfannenkuchenBot;
using Discord.WebSocket;
partial class Command
{
    public Command(SocketMessage _socketmsg)
    {
        this.channel = _socketmsg.Channel;
        this.author = _socketmsg.Author;
        this.user = Playerdata.GetPlayerdata(_socketmsg.Author.Id.ToString());
    }
    public void Daily()
    {
        user.balance += 1000;
        channel.SendMessageAsync($"Added 1000$ to your balance, which now contains {user.balance}$");
    }
    public void Balance()
    {
        channel.SendMessageAsync($"Your current balance is {user.balance}$");
    }
    readonly Playerdata user;
    readonly ISocketMessageChannel channel;
    readonly SocketUser author;
}