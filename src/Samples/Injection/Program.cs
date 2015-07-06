using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using SampleHelpers;
using Yaclops;
using Yaclops.Attributes;
using IContainer = Autofac.IContainer;
#pragma warning disable 618

namespace Injection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var container = CreateContainer();

                var parser = container.Resolve<CommandLineParser>();

                var command = parser.Parse(args);

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



        private static IContainer CreateContainer()
        {
            var settings = new CommandLineParserSettings<ISubCommand> { EnableYaclopsCommands = true };

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



    [Summary("Add things to the repo")]
    public class AddCommand : ISubCommand
    {
        [PositionalParameter]
        [Description("The item to add")]
        public string Item { get; set; }


        [NamedParameter(ShortName = "f")]
        public bool Force { get; set; }


        public void Execute()
        {
            this.Dump();
        }
    }


    [Summary("Remove things from the repo")]
    public class RemoveCommand : ISubCommand
    {
        [PositionalParameter]
        [Description("The item to remove")]
        public string Item { get; set; }


        [NamedParameter(ShortName = "f")]
        public bool Force { get; set; }


        public void Execute()
        {
            this.Dump();
        }
    }
}
