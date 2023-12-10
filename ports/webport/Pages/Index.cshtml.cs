using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PfannenkuchenBot.Commands;
using PfannenkuchenBot.Commands.Logging;

namespace PfannenkuchenBot.WebPort.Pages;
public class IndexModel : PageModel
{
    public CommandLogEntry[]? LatestResponses {get; set;}

    public void OnGet()
    {
        LatestResponses = CommandLogEntry.GetLastLogs();
    }

    [BindProperty]
    public string? Username {get;set;}
    [BindProperty]
    public string? Command {get;set;}
    public string OutPut = string.Empty;
    public void OnPost()
    {
        if (Command is null) return;
        if (Username is null) return;
        ref string s = ref OutPut;
        WebPorter.EvaluateRequest(Command, Username, s);
    }
}