using Discord.WebSocket;
namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Info(SocketMessage socketmsg, string[] commandMessage)
    {
        if (commandMessage.Length == 2 && Item.Get(commandMessage[1], out Item? item)) {
            if (item is null) throw new InvalidGameObjectException();
            message.Append(item.Describe());
        }
        else message.Append("Couldn't find the item with the id you specified, please check the spelling!");
        Send(message, socketmsg, commandMessage);
    }
    
}