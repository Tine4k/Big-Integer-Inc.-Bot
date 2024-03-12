using PfannenkuchenBot.Commands.Content.GoofyMonths;

namespace PfannenkuchenBot.Commands;
public partial class CommandHandler
{
    [Command(CommandCategory.Random)]
    public void CheckChallenge(string username)
    {
        string status = MonthChallenges.HasLost(username);
        switch (status)
        {
            case "Out":
                {
                    message.Append($"{username} has already lost the {MonthChallenges.CurrentChallengeName} :(");
                    break;
                }
            case "In":
                {
                    message.Append($"{username} is still in the {MonthChallenges.CurrentChallengeName}");
                    break;
                }
            case "NotSignedUp":
                {
                    message.Append($"{username} is not signed up for the {MonthChallenges.CurrentChallengeName}");
                    break;
                }
        }
    }

    [Command(CommandCategory.Random)]
    public void CheckChallenge()
    {
        CheckChallenge(player.Username);
    }

    [Command(CommandCategory.Random)]
    public void LoseChallenge(string username)
    {
        string status = MonthChallenges.HasLost(username);
        if (status == "In")
        {
            MonthChallenges.Lose(username);
            message.AppendLine($"@{username} lost the {MonthChallenges.CurrentChallengeName}!");
        }
        else if (status == "Out") message.AppendLine($"{username} has already lost the {MonthChallenges.CurrentChallengeName} :(");
        else if (status == "NotSignedUp") message.AppendLine($"{username} hasn't signed up for the {MonthChallenges.CurrentChallengeName}");
    }

    [Command(CommandCategory.Random)]
    public void LoseChallenge()
    {
        LoseChallenge(player.Username);
    }

    [Command(CommandCategory.Random)]
    public void SignUp(string username)
    {
        string status = MonthChallenges.HasLost(username);
        if (status == "In") message.Append($"{username} is already signed up, he is still in the {MonthChallenges.CurrentChallengeName}");
        else if (status == "Out") message.Append($"{username} is signed up, but he already failed the {MonthChallenges.CurrentChallengeName}");
        else if (status == "NotSignedUp")
        {
            MonthChallenges.SignUp(username);
            message.Append($"{username} is now signed up for the {MonthChallenges.CurrentChallengeName}!");
        }
    }

    [Command(CommandCategory.Random)]
    public void SignUp()
    {
        SignUp(player.Username);
    }
}