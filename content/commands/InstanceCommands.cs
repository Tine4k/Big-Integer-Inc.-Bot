namespace PfannenkuchenBot;
partial class Command
{
    public void Daily()
    {
        playerdata.balance += 1000;
        channel.SendMessageAsync($"Added 1000$ to your balance, which now contains {playerdata.balance}$");
    }
    public void Balance()
    {
        channel.SendMessageAsync($"Your current balance is {playerdata.balance}$");
    }
    public void Clear()
    {
        playerdata.inventory.Clear();
    }   
}