using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Yaclops.Help
{
    internal class HelpCommand : ISubCommand
    {
        private readonly CommandLineParser _parser;


        public HelpCommand(CommandLineParser parser)
        {
            _parser = parser;
            Commands = new List<string>();
        }


        [CommandLineParameter]
        public List<string> Commands { get; private set; }


        public void Execute()
        {
            IConsole console = new WrappedConsole();

            // TODO - allow the name to be overridden in settings
            string exeName = Assembly.GetEntryAssembly().GetName().Name.ToLower();

            if (Commands.Any())
            {
                ISubCommand command;
                try
                {
                    command = _parser.Parse(Commands);
                }
                catch (CommandLineParserException)
                {
                    console.WriteLine("Information on that command is not available, as it does not exist.");
                    return;
                }

                // TODO - how to get help on multi-word commands, like bisect? ('sample help bisect' should show *something*)

                // Print the synopsis
                var type = command.GetType();

                console.StartWrap("usage: {0} {1}", exeName, command.Name());

                // TODO - repeat global flags?

                // Write out the named parameters
                var namedParams = type.GetProperties()
                    .Where(x => x.GetCustomAttributes(typeof(CommandLineOptionAttribute), true).Any());

                foreach (var p in namedParams)
                {
                    var att = (CommandLineOptionAttribute)p.GetCustomAttributes(typeof(CommandLineOptionAttribute), true).First();
                    List<string> opts = new List<string>();

                    if (!string.IsNullOrEmpty(att.LongName))
                    {
                        opts.Add("--" + att.LongName);
                    }
                    else
                    {
                        opts.Add("--" + p.Name.Decamel('-'));                        
                    }

                    if (!string.IsNullOrEmpty(att.ShortName))
                    {
                        opts.Add("-" + att.ShortName);
                    }

                    console.Write(" [{0}]", string.Join(" | ", opts));
                }

                // Write out the positional parameters
                var positional = type.GetProperties()
                    .Where(x => x.GetCustomAttributes(typeof (CommandLineParameterAttribute), true).Any());

                foreach (var p in positional)
                {
                    // TODO - for lists, indicate mmultiple
                    console.Write(" [<{0}>]", p.Name.Decamel());
                }

                // Finish off the synopsis
                console.EndWrap();
                console.WriteLine();

                console.WriteLine(command.Summary());

                // TODO - long description
                console.WriteLine(command.Description());

                // TODO - option details
            }
            else
            {
                console.StartWrap("usage: {0}", exeName);

                // TODO - write global flags

                console.Write(" <command> [<args>]");

                console.EndWrap();
                console.WriteLine();

                HelpBuilder.WriteCommandList(_parser.Commands, console);
            }
        }
    }
}
