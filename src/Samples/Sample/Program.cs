using System;
using System.Diagnostics;
using System.Reflection;
using Autofac;
using Yaclops;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var container = CreateContainer();

                var globals = new GlobalSettings();

                var parser = container.Resolve<CommandLineParser>();
                parser.AddGlobalOptions(globals);

                var command = parser.Parse(args);

                if (globals.ShowLogo)
                {
                    PrintLogo();
                }

                if (command != null)
                {
                    command.Execute();
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



        private static IContainer CreateContainer()
        {
            var settings = new CommandLineParserSettings<ISubCommand>();
            settings.EnableYaclopsCommands = true;

            ContainerBuilder builder = new ContainerBuilder();

            // Command-line specific stuff
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => typeof(ISubCommand).IsAssignableFrom(t) && t.IsPublic)
                .SingleInstance()
                .As<ISubCommand>();

            builder.RegisterType<CommandLineParser>().WithParameter("settings", settings);

            return builder.Build();
        }
    }
}
