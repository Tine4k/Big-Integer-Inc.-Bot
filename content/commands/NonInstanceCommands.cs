using Discord;
using Discord.WebSocket;
namespace PfannenkuchenBot.Commands;
class NonInstanceCommand : Command
{
    // * Any commands that don't require the reference of playerdata belong here
    public NonInstanceCommand(SocketMessage _socketmsg, string[] _command) : base(_socketmsg, _command)
    { }
    public void Help()
    {
        message.Append(Format.Bold("List of all available commands: (Not available yet)"));
        Send();
    }
    public void HelloWorld()
    {
        message.Append("Hello There");
        Send();
    }
    public void Jone()
    {
        message.Append(
        Format.BlockQuote(Format.Bold(
        "Rauchst du an Jone, bist du da Mane!\nRauchst du kan, brauchst du an!\n") +
        "Kontaktieren Sie die Jone GmbH gerne unter " +
        Format.Italics(Format.Underline("+43 067689807553"))));
        Send();
    }
    public void Info()
    {        
        if (command.Length >= 2 && Item.Get(command[1], out Item item)) {
            message.Append(item.Describe);
        }
    }
}