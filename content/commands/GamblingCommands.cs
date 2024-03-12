namespace PfannenkuchenBot.Commands;

public partial class CommandHandler{
    [Command(CommandCategory.Economy)]
    public void Coinflip(string bet, uint amount)
    {
        if (player.Balance < amount) message.Append("Bro your broke, dont try to scam me!");
        else if ( bet.Equals("Heads", StringComparison.OrdinalIgnoreCase) || 
        bet.Equals("Kopf", StringComparison.OrdinalIgnoreCase) || 
        bet.Equals("h", StringComparison.OrdinalIgnoreCase) || 
        bet.Equals("k", StringComparison.OrdinalIgnoreCase) || 
        bet.Equals("Head", StringComparison.OrdinalIgnoreCase))
        {
            if (Random.Shared.Next(2) == 0) {
                message.Append($"The coin landed on **Heads**, you win {amount}{Config.currency}! ");
                player.Gain(amount);
            }
            else {
                message.Append($"The coin landed on **Tails**, you lose your bet of {amount}{Config.currency}! ");
                player.ForceLose(amount);
            }
            message.AppendLine($"Now you have {player.Balance}{Config.currency}, make sure to reinvest it ASAP!");
        }
        else if ( bet.Equals("Tails", StringComparison.OrdinalIgnoreCase) || 
        bet.Equals("Zahl", StringComparison.OrdinalIgnoreCase) || 
        bet.Equals("t", StringComparison.OrdinalIgnoreCase) || 
        bet.Equals("z", StringComparison.OrdinalIgnoreCase) || 
        bet.Equals("Tail", StringComparison.OrdinalIgnoreCase))
        {
            if (Random.Shared.Next(2) == 0) 
            {
                message.Append($"The coin landed on **Tails**, you win {amount}{Config.currency}! ");
                player.Gain(amount);
            }
            else 
            {
                message.Append($"The coin landed on **Heads**, you lose your bet of {amount}{Config.currency}! ");
                player.ForceLose(amount);
            }
            message.AppendLine($"Now you have {player.Balance}{Config.currency}, but if you play now, your loss will be no more!");
        }
        else message.Append("Your request was invalid!");
    }
}