namespace PfannenkuchenBot.WebPort.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PfannenkuchenBot.Commands.Logging;

public class IndexModel : PageModel
{
    public List<CommandLogEntry>? LatestResponses {get; set;}

    public void OnGet()
    {
        LatestResponses = GetLastLogs();


        static List<CommandLogEntry> GetLastLogs()
        {
            List<CommandLogEntry> Responses = new();
            using (StreamReader reader = new(Logger.currentLogPath))
            {
                for (int i=0; i<10 && !reader.EndOfStream;i++) Responses.Add(CommandLogEntry.Parse(reader.ReadLine()!));
                
            };
            return Responses;
        }
    }

    [BindProperty]
    public string? Username {get;set;}

    public void OnPost()
    {

    }
}