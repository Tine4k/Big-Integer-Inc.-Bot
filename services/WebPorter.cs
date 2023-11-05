using PfannenkuchenBot.Commands;

namespace PfannenkuchenBot.WebPort;
public class WebPorter : IPorter
{
    public static void StartUp()
    {
        CreateHostBuilder();
    }
    static IHostBuilder CreateHostBuilder() =>
    Host.CreateDefaultBuilder()
        .ConfigureWebHost(
            webHost => webHost
                .UseKestrel(kestrelOptions => { kestrelOptions.ListenAnyIP(5200); })
                .Configure(app => app
                    .Run(
                        async context =>
                        {
                            await context.Response.WriteAsync("Hello World!");
                        }
                    )));

    public static async Task SendAsync(string message, object context)
    {
        
    }
}