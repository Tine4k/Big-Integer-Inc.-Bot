using System.Runtime.CompilerServices;

namespace PfannenkuchenBot.Commands.Content.GoofyMonths;
public static class Challenges
{
    static Challenges()
    {
        CurrentChallengeName = "No Nazi November";
    }
    public static string CurrentChallengeName {get;}
    static string[] ChallengeParticipipants {get => File.ReadAllLines("saves/");}
    public static bool SignUp(string username)
    {
        if (ChallengeParticipipants.Contains(username)) return false;
        else return true;
    }
}