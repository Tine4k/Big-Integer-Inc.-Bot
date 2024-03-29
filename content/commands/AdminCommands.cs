namespace PfannenkuchenBot.Commands;
public partial class CommandHandler
{
    [Command(CommandCategory.Admin)]
    public void Give()
    {
        if (currentCommandMessage.Length == 2)
        {
            try
            {
                player.Gain(currentCommandMessage[1]);
                message.Append($"Gave you {currentCommandMessage[1]}{Config.currency}.");
            }
            catch
            {
                if (uint.TryParse(currentCommandMessage[1], out uint amount)) 
                {
                    player.Gain(amount);
                   message.Append($"Gave you {currentCommandMessage[1]}{Config.currency}.");
                }
                else message.Append("Invalid arguments.");
            }
        }
        else if (
            currentCommandMessage.Length >= 3 &&
            uint.TryParse(currentCommandMessage[2], out uint amount)
            )
        {
            player.Gain(currentCommandMessage[1], amount);
            message.Append($"Gave you {amount}x {currentCommandMessage[1]}");
        }
        else message.Append("Wasn't able to give you this item.");
    }
}