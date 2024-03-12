namespace PfannenkuchenBot.Commands.Content.GoofyMonths;

public static class MonthChallenges
{
    static MonthChallenges()
    {
        CurrentChallengeName = "No Nazi November";
        CurrentChallengeSave = $"saves/nonazinovember.txt";
    }
    public static string CurrentChallengeName {get;} 
    static string CurrentChallengeSave {get;} 
    static string[] ChallengeParticipants {get
    {
        if (File.Exists(CurrentChallengeSave)) return File.ReadAllLines(CurrentChallengeSave);
        File.Create(CurrentChallengeSave);
        return Array.Empty<string>();
    }
    
    }
    public static void Lose(string username)
    {
        string[] participants = ChallengeParticipants;
        int index = Array.IndexOf(participants, username);
        if (index == -1 ) return;
        participants[index] = $"~~{username}~~";
        File.WriteAllLines(CurrentChallengeSave, participants);
    }
    public static void SignUp(string username)
    {
        if (!File.Exists(CurrentChallengeSave)) File.Create(CurrentChallengeSave);
        File.WriteAllText(CurrentChallengeSave, $"\n{username}");
    }
    public static string HasLost(string username)
    {
        string[] participants = ChallengeParticipants;
        if (participants.Contains(username)) return "In";
        if (participants.Contains($"~~{username}~~")) return "Out";
        else return "NotSignedUp";
    }
}