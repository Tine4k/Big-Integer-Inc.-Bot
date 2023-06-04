using Discord;
using Discord.WebSocket;
namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Help(SocketMessage socketmsg, string[] commandMessage)
    {
        message.Append(Format.Bold("List of all available commands: (Not available yet)"));
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
        Format.BlockQuote(Format.Bold(
        "Rauchst du an Jone, bist du da Mane!\nRauchst du kan, brauchst du an!\n") +
        "Kontaktieren Sie die Jone GmbH gerne unter " +
        Format.Italics(Format.Underline("+43 067689807553"))));
        Send(message, socketmsg, commandMessage);
    }
    public void Info(SocketMessage socketmsg, string[] commandMessage)
    {
        if (commandMessage.Length >= 2 && Item.Get(commandMessage[1], out Item? item)) {
            if (item is null) {Unknown(socketmsg.Channel); return;}
            message.Append(item.Describe());
        }
        Send(message, socketmsg, commandMessage);
    }
}