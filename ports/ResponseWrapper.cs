namespace PfannenkuchenBot;
public class ResponseWrapper
{
    public string? Response { get; set; }
    public override string ToString()
    {
        return Response ?? string.Empty;
    }
}