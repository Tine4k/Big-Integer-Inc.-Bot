using Microsoft.AspNetCore.Mvc.RazorPages;
using PfannenkuchenBot.Commands;
namespace PfannenkuchenBot.WebPort;
public class WebPorter : IPorter
{
    public static void StartUp()
    {
        var host = new WebHostBuilder()
            .UseKestrel(kestrelOptions => { kestrelOptions.ListenLocalhost(5005); })
            .UseWebRoot(@$"{Environment.CurrentDirectory}\wwwroot")
            .UseContentRoot(@$"{Environment.CurrentDirectory}\ports\webport")
            .UseStartup<Startup>()
            .Build();
        host.Run();
    }

    public static Task SendAsync(string message, object context)
    {
        return Task.CompletedTask;
    }


    private class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddRazorPages(options => 
            {
                options.RootDirectory = @$"/ports/webport/Pages";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }

}