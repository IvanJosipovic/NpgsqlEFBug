using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NpgsqlEFBug
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                            options2 => options2.EnableRetryOnFailure()));

                    services.AddScoped<Worker>();

                    try
                    {
                        var context = services.BuildServiceProvider().GetRequiredService<ApplicationDbContext>();
                        context.Database.EnsureCreated();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Database EnsureCreated: " + ex);
                    }

                    services.BuildServiceProvider().GetService<Worker>().StartAsync(new System.Threading.CancellationToken());
                });
    }
}
