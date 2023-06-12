using Discord.WebSocket;
using Pfannenkuchenbot.Item;

namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Mane(SocketMessage socketmsg, string[] commandMessage)
    {
        playerdata.Gain("jone", (uint)Random.Shared.Next(0,1));
        message.Append("@Klagenfurt Busbahnhof");
        Send(message, socketmsg, commandMessage);
    }

    public void Jone(SocketMessage socketmsg, string[] commandMessage)
    {
        message.Append(
        """
        ```
        **Rauchst du an Jone, bist du da Mane! Rauchst du kan, brauchst du an!**
        Kontaktieren Sie die Jone GmbH gerne unter: 
        __**+43 067689807553**__
        ```
        """);
        Send(message, socketmsg, commandMessage);
    }
}