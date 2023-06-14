using Discord.WebSocket;

namespace PfannenkuchenBot.Commands;
partial class Command
{
    public void Daily()
    {
        if (!playerdata.Timestamps.ContainsKey("lastDaily")) playerdata.Timestamps.Add("lastDaily", DateTime.Today - TimeSpan.FromDays(1));
        if (playerdata.Timestamps["lastDaily"].Date == DateTime.Today.Date)
        {
            TimeSpan timeTillNextDay = (DateTime.Today + (TimeSpan.FromDays(1))) - DateTime.Now;
            message.Append($"Wait **{timeTillNextDay.Hours}h {timeTillNextDay.Minutes}min and {timeTillNextDay.Seconds}s** to be able to get your next daily reward!");
        }
        else
        {
        playerdata.Gain(1000);
        message.Append($"Added 1000{Config.currency} to your balance, which now contains {playerdata.Balance}{Config.currency}");
        }
        playerdata.Timestamps["lastDaily"] = DateTime.Today;
        
    }
}