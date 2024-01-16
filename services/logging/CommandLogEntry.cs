namespace PfannenkuchenBot.Commands.Logging;
public class CommandLogEntry
{
    public CommandLogEntry(string Content, string Command, string Target, DateTime TimeStamp, CommandTargeting Targeting)
    {
        this.Content = Content;
        this.Command = Command;
        this.targets = new string[] { Target };
        this.TimeStamp = TimeStamp;
        this.Targeting = Targeting;
    }

    public static CommandLogEntry[] GetLastLogs(string? path = null, int count = 10)
    {
        List<CommandLogEntry> Responses = new();
        path ??= Logger.currentLogPath;

        using (StreamReader reader = new(path))
        for (int i = 0; i < count && !reader.EndOfStream;)
        {
            string? logEntry = reader.ReadLine();
            if (logEntry is null) continue;
            if (logEntry[9..17] != "Commands") continue;
            string[] subString = logEntry.Split(new string[]{" Commands "," issued "}, StringSplitOptions.None);
            int colonIndex = subString[2].IndexOf(':');
            CommandTargeting targeting;
            char targetingFlag = logEntry[19];
            if (targetingFlag == ' ') targeting = CommandTargeting.Issuer;
            else if (targetingFlag == 'M') targeting = CommandTargeting.Multiple;
            else if (targetingFlag == 'G') targeting = CommandTargeting.Global;
            else targeting = CommandTargeting.Issuer;
            Responses.Add(new CommandLogEntry(subString[2][..colonIndex], subString[2][colonIndex..].Trim('"'), subString[1], DateTime.Parse(subString[0]), targeting));
            ++i;
        }
        
        return Responses.ToArray();
    }

    readonly string[] targets;
    public string Target { get => targets[0]; }
    public string[] Targets { get => targets; }
    public string Command { get; init; }
    public string Content { get; init; }
    public DateTime TimeStamp { get; init; }
    public CommandTargeting Targeting { get; init; }
}