namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Give()
    {
        if (currentCommandMessage.Length == 2)
        {
            try
            {
                playerdata.Gain(currentCommandMessage[1]);
                message.Append($"Gave you {currentCommandMessage[1]}.");
            }
            catch
            {
                if (uint.TryParse(currentCommandMessage[1], out uint amount)) 
                {
                    playerdata.Gain(amount);
                   message.Append($"Gave you {currentCommandMessage[1]}.");
                }
                else message.Append("Invalid arguments.");
            }
        }
        else if (
            currentCommandMessage.Length >= 3 &&
            uint.TryParse(currentCommandMessage[2], out uint amount)
            )
        {
            playerdata.Gain(currentCommandMessage[1], amount);
            message.Append($"Gave you {amount}x {currentCommandMessage[1]}");
        }
        else message.Append("Wasn't able to give you this item.");
    }
}