using Discord;
using Discord.WebSocket;
namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Help(SocketMessage socketmsg, string[] commandMessage)
    {
        message.Append("**List of all available commands: (Not available yet)**");
        Send(message, socketmsg, commandMessage);
    }
    public void HelloWorld(SocketMessage socketmsg, string[] commandMessage)
    {
        message.Append("Hello There");
        Send(message, socketmsg, commandMessage);
    }
    public void Jone(SocketMessage socketmsg, string[] commandMessage)
    {
        message.Append(
        Format.BlockQuote(
        """
        **Rauchst du an Jone, bist du da Mane! Rauchst du kan, brauchst du an!**
        Kontaktieren Sie die Jone GmbH gerne unter: 
        __**+43 067689807553**__
        """));
        Send(message, socketmsg, commandMessage);
    }
    public void Info(SocketMessage socketmsg, string[] commandMessage)
    {
        if (commandMessage.Length == 2 && Item.Get(commandMessage[1], out Item? item)) {
            if (item is null) {Unknown(socketmsg.Channel); return;}
            message.Append(item.Describe());
        }
        else message.Append("That's not a item I know.");
        Send(message, socketmsg, commandMessage);
    }
    public void Shop(SocketMessage socketmsg, string[] commandMessage)
    {
        foreach(Item item in GameElement.ItemLoader.loadedInstances.Values)
        {
            if(item.BuyPrice != 0 && !(item.Tags.Contains("Illegal")))
            {
                message.Append($"{item.Name}: {item.BuyPrice}{Config.currency}\n\n");
            }            
        }
        Send(message, socketmsg, commandMessage);
    }
    public void BlackMarket(SocketMessage socketmsg, string[] commandMessage)
    {
        message.Append("**Welcome to the Black Market!\n**");
        foreach(Item item in GameElement.ItemLoader.loadedInstances.Values)
        {
            if(item.BuyPrice != 0 && item.Tags.Contains("Illegal"))
            {
                message.Append($"{item.Name}: {item.BuyPrice}{Config.currency}\n\n");
            }
        }
        Send(message, socketmsg, commandMessage);
    }
}