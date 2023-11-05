using Discord.WebSocket;

namespace PfannenkuchenBot.Commands;
public partial class CommandHandler
{
    [Command(CommandCategory.Rewards)]
    public void Daily()
    {
        if (!player.Timestamps.ContainsKey("lastDaily")) player.Timestamps.Add("lastDaily", DateTime.Today - TimeSpan.FromDays(1));
        if (player.Timestamps["lastDaily"].Date == DateTime.Today.Date)
        {
            TimeSpan timeTillNextDaily = (DateTime.Today + (TimeSpan.FromDays(1))) - DateTime.Now;
            message.Append($"Wait **{((timeTillNextDaily.Hours > 0) ? (timeTillNextDaily.Hours + "h ") : "")}{timeTillNextDaily.Minutes}min and {timeTillNextDaily.Seconds}s** to be able to get your next daily reward!");
        }
        else
        {
        player.Gain(1000);
        message.Append($"Added 1000{Config.currency} to your balance, which now contains {player.Balance}{Config.currency}");
        }
        player.Timestamps["lastDaily"] = DateTime.Today;
        
    }
}