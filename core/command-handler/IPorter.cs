namespace PfannenkuchenBot.Commands;
public interface IPorter
{
    public static abstract Task Send(string message, object context);
}