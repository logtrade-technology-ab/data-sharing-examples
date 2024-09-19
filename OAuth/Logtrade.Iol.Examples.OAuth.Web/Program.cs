using Logtrade.Iol.Examples.OAuth.Core;
using Logtrade.Iol.Examples.OAuth.Core.Services;

namespace Logtrade.Iol.Examples.OAuth.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.AddConsole();

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        Env.Settings = builder.Configuration.GetSection("Settings")!.Get<Settings>()!;

        builder.Services.AddHttpClient();
        builder.Services.AddTransient<IolOAuthConnector>();
        builder.Services.AddSingleton<ExampleRepository>();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}