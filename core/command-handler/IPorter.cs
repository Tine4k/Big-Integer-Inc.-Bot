namespace PfannenkuchenBot.Commands;
public interface IPorter
{
    public static abstract Task SendAsync(string message, object context);
}