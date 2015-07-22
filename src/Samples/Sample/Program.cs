using System;
using System.Diagnostics;
using System.Reflection;
using Sample.Commands;
using SampleHelpers;
using Yaclops;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var globals = new GlobalSettings();

                var settings = new CommandLineParserSettings<ISampleCommand>
                {
                    ProgramName = "MyApp"
                };

                var parser = ParserBuilder<ISampleCommand>.FromCommands(Assembly.GetExecutingAssembly())
                    .WithGlobals(globals)
                    .WithSettings(settings)
                    .Build();

                var command = parser.Parse(args);

                if (globals.ShowLogo)
                {
                    PrintLogo();
                }

                if (command != null)
                {
                    command.Dump();
                }
            }
            catch (CommandLineParserException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception in main.");
                Console.WriteLine(ex);
            }

            if (Debugger.IsAttached)
            {
                Console.Write("<press ENTER to continue>");
                Console.ReadLine();
            }
        }



        private static void PrintLogo()
        {
            // From the most excellent site: http://www.patorjk.com/software/taag
            const string logo = @"
  ____                        _      
 / ___|  __ _ _ __ ___  _ __ | | ___ 
 \___ \ / _` | '_ ` _ \| '_ \| |/ _ \
  ___) | (_| | | | | | | |_) | |  __/
 |____/ \__,_|_| |_| |_| .__/|_|\___|
                       |_|           
";

            Console.WriteLine(logo);
        }
    }
}
