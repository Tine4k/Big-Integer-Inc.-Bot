using Discord.WebSocket;
using Pfannenkuchenbot.Item;

namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Give(SocketMessage socketmsg, string[] commandMessage)
    {
        if (commandMessage.Length == 2) playerdata.Gain(commandMessage[1]);
        else if (
            commandMessage.Length >= 3 &&
            uint.TryParse(commandMessage[2], out uint amount)
            ) playerdata.Gain(commandMessage[1], amount);
        else Unknown(socketmsg.Channel);
    }
}