namespace PfannenkuchenBot.Commands.Logging;
public class CommandLogEntry
{
    public static CommandLogEntry Parse(string text)
    {
        throw new NotImplementedException();
    }

    public required string Content {get;init;}
    public required List<Playerdata> Targets {get;init;}
    public required DateTime TimeStamp {get;init;}
    public bool IsGlobal {get;init;}
}