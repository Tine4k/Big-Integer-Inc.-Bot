namespace PfannenkuchenBot.Commands;
partial class Command
{
    [Command(CommandCategory.Random, Cooldown = 10)]
    public void Mane(string Location)
    {
        switch (Location)
        {
            case "Völkermarkt" or "Voelkermarkt":
                {
                    player.Gain("jone", (uint)Random.Shared.Next(0, 1));
                    message.Append("**@Völkermarkt Sumsipark**");
                    break;
                }
            case "Heiliger" or "Heiligengeistplatz":
                {
                    player.Gain("jone", (uint)Random.Shared.Next(0, 1));
                    message.Append("**@Am Heiligen um dreie in da fruah**");
                    break;
                }
            default:
                {
                    player.Gain("jone", (uint)Random.Shared.Next(0, 1));
                    message.Append("**@Klagenfurt Busbahnhof**");
                    break;
                }
        }

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