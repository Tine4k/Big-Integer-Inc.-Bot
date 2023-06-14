namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Give()
    {
        if (commandMessage.Length == 2)
        {
            try
            {
                playerdata.Gain(commandMessage[1]);
                message.Append($"Gave you {commandMessage[1]}.");
            }
            catch
            {
                if (uint.TryParse(commandMessage[1], out uint amount)) 
                {
                    playerdata.Gain(amount);
                   message.Append($"Gave you {commandMessage[1]}.");
                }
                else message.Append("Invalid arguments.");
            }
        }
        else if (
            commandMessage.Length >= 3 &&
            uint.TryParse(commandMessage[2], out uint amount)
            )
        {
            playerdata.Gain(commandMessage[1], amount);
            message.Append($"Gave you {amount}x {commandMessage[1]}");
        }
        else message.Append("Wasn't able to give you this item.");
    }
}