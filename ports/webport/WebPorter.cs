using PfannenkuchenBot.Commands;

namespace PfannenkuchenBot.WebPort;
public class WebPorter : IPorter
{
    public static void StartUp()
    {
        var host = new WebHostBuilder()
            .UseKestrel(kestrelOptions => { kestrelOptions.ListenAnyIP(5005); })
            .UseContentRoot(@"ports\webport")
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
        // This method is used to configure services (dependencies) for the application.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services, such as MVC, Entity Framework, authentication, etc.
            services.AddMvc();
        }

        // This method is used to configure the request processing pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Configure routing, static files, and other middleware components.
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Add controllers and set up default routing.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

}