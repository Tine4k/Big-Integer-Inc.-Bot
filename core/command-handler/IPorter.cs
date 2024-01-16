namespace PfannenkuchenBot.Commands;
public interface IPorter
{
    public static abstract Task Send(string message, object context);
    // public static abstract string Format(string message);
}