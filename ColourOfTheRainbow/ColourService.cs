using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace ColourOfTheRainbow
{
    public class ColourService : IColourService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ColourService> _log;
        private string _input;
        private IList<Colour> _colours;

        public ColourService(ILogger<ColourService> log, IConfiguration configuration)
        {
            _log = log;
            _configuration = configuration;
        }

        public void CheckInput(string[] args)
        {
            if (args.Length == 0)
            {
                _log.LogError("No Arguments Passed");
                Console.WriteLine("Please try again and provide a name of a colour in the rainbow.\nThis application will shutdown in 3 seconds");
                Thread.Sleep(3000);
                Environment.Exit(0);
                return;
            }

            if (args.Length > 1)
            {
                _log.LogError("To Many Arguments Passed");
                Console.WriteLine("Only one input at a time, please try again.\nThis application will shutdown in 3 seconds");
                Thread.Sleep(3000);
                Environment.Exit(0);
                return;
            }

            if (args[0] is not string || !Regex.IsMatch(args[0], @"^[a-zA-Z]+$"))
            {
                _log.LogError("Non Alphabetic Arguments Passed");
                Console.WriteLine("Only alphabetic input are accepted\nThis application will shutdown in 3 seconds");
                Thread.Sleep(3000);
                Environment.Exit(0);
                return;
            }

            _input = args[0];
        }

        public void ProvideColourCode()
        {
            var returnType = _configuration.GetValue<string>("ReturnType") != null ? _configuration.GetValue<string>("ReturnType") : ""; 

            Colour rainbowColour = _colours.FirstOrDefault(colour => colour.Name == _input);
            
            if (rainbowColour == null)
            {
                _log.LogError("Colour Was Not Found In Dictionary");
                Console.WriteLine("Colour Not Found. Only colours in the rainbow are accepted");
                Console.WriteLine("For Exampled");
                foreach (Colour colour in _colours)
                {
                    Console.WriteLine($"{colour.Name}");
                }
                Console.WriteLine("This application will shutdown in 3 seconds");
                Thread.Sleep(3000);
                Environment.Exit(0);
                return;
            }

            string colourCode = returnType.ToUpper() == "HEX" || (returnType.ToUpper() != "HEX" && returnType.ToUpper() != "RGB") ? rainbowColour.Hex : rainbowColour.RGB;

            if (colourCode == null)
            {
                _log.LogError("Colour Did Not Contain A {returnType} value", returnType);
                Console.WriteLine($"This colour has not been configured with {returnType} Value");
                Console.WriteLine("This application will shutdown in 3 seconds");
                Thread.Sleep(3000);
                Environment.Exit(0);
                return;
            }
            _log.LogError("Application Successfully Return {returnType}: {colourCode} for {_input}", returnType, colourCode, _input );
            Console.WriteLine(colourCode);
        }

        public void CreateColourListFromConfig()
        {
            IList<Colour> coloursDict = _configuration.GetSection("Colours").Get<IList<Colour>>();
            if (coloursDict == null && coloursDict.Any())
            {
                _log.LogError("Application Missing Configuration File Or Colours Configured Incorrectly");
                Console.WriteLine("This application is missing a configuration file or has been configured incorrectly");
                Console.WriteLine("This application will shutdown in 3 seconds");
                Thread.Sleep(3000);
                Environment.Exit(0);
                return;
            }
            
            _colours = coloursDict;
        }
    }
}