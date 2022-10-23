using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ColourOfTheRainbow
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            BuildConfigeration(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.File($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/ColourOfRainbowLog.txt")
                .CreateLogger();

            Log.Logger.Information("Application Starting With Input Of:");
            foreach (string input in args)
            {
                Log.Logger.Information("{input}", input);
            }

            IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IColourService, ColourService>();

                })
                .UseSerilog()
                .Build();

            

            ColourService colourColour = ActivatorUtilities.CreateInstance<ColourService>(host.Services);
            colourColour.CheckInput(args);
            colourColour.CreateColourListFromConfig();
            colourColour.ProvideColourCode();

        }

        static void BuildConfigeration(IConfigurationBuilder builder)
        {
            // Builds the JSON configuration file and allows enironmental file and varaibles to override the default 
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Productino"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}