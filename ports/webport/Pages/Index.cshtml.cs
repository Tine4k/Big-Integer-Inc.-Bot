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
    public readonly ResponseWrapper responseWrapper = new();
    public void OnPost()
    {
        if (Command is null || Username is null) responseWrapper.Response += "Values are unspecified";
        else WebPorter.EvaluateRequest(Command, Username, responseWrapper);
    }
}