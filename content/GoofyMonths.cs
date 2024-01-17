namespace PfannenkuchenBot.Commands.Content.GoofyMonths;
public static class Challenges
{
    static Challenges()
    {
        CurrentChallengeName = "No Nazi November";
        CurrentChallengeSave = $"saves/nonazinovember.txt";
    }
    public static string CurrentChallengeName {get;} 
    static string CurrentChallengeSave {get;} 
    static string[] ChallengeParticipipants {get => File.ReadAllLines(CurrentChallengeSave);}
    public static void Loose(string username)
    {
        File.WriteAllText(CurrentChallengeSave, $"\n{username}");
    }
    public static bool HasLost(string username)
    {
        if (ChallengeParticipipants.Contains(username)) return true;
        else return false;
    }
}