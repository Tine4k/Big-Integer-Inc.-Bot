using Discord;
using Discord.WebSocket;
using Pfannenkuchenbot.Item;

namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Mane()
    {
        player.Gain("jone", (uint)Random.Shared.Next(0,1));
        message.Append("@Klagenfurt Busbahnhof");
        
    }

    public void Jone()
    {
        message.Append(
        """
        >>> **Rauchst du an Jone, bist du da Mane! Rauchst du kan, brauchst du an!**
        Kontaktieren Sie die Jone GmbH gerne unter: 
        __**+43 067689807553**__
        """);
        
    }
}