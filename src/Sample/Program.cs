#define NEW_PARSER

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

#if NEW_PARSER
                var parser = container.Resolve<CommandLineParserEx>();
#else
                var parser = container.Resolve<CommandLineParser>();
#endif

                var command = parser.Parse(args);

                command.Execute();
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



        private static IContainer CreateContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Command-line specific stuff
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => typeof(ISubCommand).IsAssignableFrom(t) && t.IsPublic)
                .SingleInstance()
                .As<ISubCommand>();

#if NEW_PARSER
            builder.RegisterType<CommandLineParserEx>();
#else
            builder.RegisterType<CommandLineParser>();
#endif

            return builder.Build();
        }
    }
}
