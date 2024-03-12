using PfannenkuchenBot.Commands;

namespace PfannenkuchenBot.ConsolePort;

public class ConsolePorter : IPorter
{
    public static DateTime lastUsed;
    public static void StartUp()
    {
        lastUsed = DateTime.Now;
        string? username = null;
        Console.WriteLine("Enter your username:");
        while (true)
        {
            username = Console.ReadLine();
            if (username is null || username.Length < 2 || username.Length > 32) Console.WriteLine("Please enter a valid username!");
            else
            {
                username = username.ToLower();
                Console.WriteLine($"Successfully logged in as \"{username}\"!");
                break;
            }
        }
        
        while (true)
        {
            string? message = Console.ReadLine();
            lastUsed = DateTime.Now;
            if (message is null) continue;
            if (message.Equals("stop", StringComparison.OrdinalIgnoreCase)) break;
            string[] commandMessage = message.Split(' ');
            CommandHandler.HandleCommand<ConsolePorter>(commandMessage, username!, new ResponseWrapper());
        }
    }
    public static Task Send(string message, object context)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}