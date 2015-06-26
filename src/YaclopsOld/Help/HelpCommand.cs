using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Yaclops.Attributes;
using Yaclops.Parsing.Configuration;

namespace Yaclops.Help
{
    /// <summary>
    /// Internal help command.
    /// </summary>
    [Summary("Provide help on the commands and options.")]
    // TODO - allow help aliases to be overridden via settings
    // TODO - add description
    internal class HelpCommand : ISubCommand
    {
        private readonly ParserConfiguration _configuration;
        private readonly IConsole _console;

        public HelpCommand(ParserConfiguration configuration, IConsole console)
        {
            _configuration = configuration;
            _console = console;
        }


        [PositionalParameter]
        public List<string> Commands { get; set; }


        public ParserCommand Target { get; set; }


        public void Execute()
        {
            if (Target == null)
            {
                ListCommands();
            }
            else
            {
                OneCommand();
            }
        }


        private void ListCommands()
        {
            _console.StartWrap("usage: {0}", ExeName());

            // TODO - write global flags

            _console.Write(" <command> [<args>]");

            _console.EndWrap();
            _console.WriteLine();

            int maxLength = _configuration.Commands.Select(x => x.Text.Length).Max();
            foreach (var command in _configuration.Commands.OrderBy(x => x.Text))
            {
                _console.WriteLine("   {0}   {1}", command.Text.PadRight(maxLength), command.Summary);
            }

            _console.WriteLine();
            _console.WriteLine("See '{0} help <command>' to read about a specific subcommand.", ExeName());
        }



        private void OneCommand()
        {
            // Start off with a blank line...seems cleaner to me...
            _console.WriteLine();

            // Print the name
            _console.WriteTitle("Name");
            _console.WriteLine("{0} {1} - {2}", ExeName(), Target.Text, Target.Summary);

            // Print the synopsis
            _console.WriteLine();
            _console.WriteTitle("Synopsis");
            _console.StartWrap("{0} {1}", ExeName(), Target.Text);

            // TODO - repeat global flags?

            // Write out the named parameters
            foreach (var param in Target.NamedParameters)
            {
                List<string> opts = new List<string>();

                opts.AddRange(param.LongNames.Select(x => "--" + x));
                opts.AddRange(param.ShortNames.Select(x => "-" + x));

                _console.Write(" [{0}]", string.Join(" | ", opts));
            }

            // Write out the positional parameters
            foreach (var param in Target.PositionalParameters)
            {
                List<string> bits = new List<string>();

                bits.Add(" ");

                if (!param.IsRequired)
                {
                    bits.Add("[");
                }

                bits.Add("<");
                bits.Add(param.Name);
                bits.Add(">");

                if (!param.IsRequired)
                {
                    bits.Add("]");
                }

                // TODO - for lists, indicate multiple
                _console.Write(string.Concat(bits));
            }

            // Finish off the synopsis
            _console.EndWrap();

            // Long description
            if (!string.IsNullOrEmpty(Target.Description))
            {
                _console.WriteLine();
                _console.WriteTitle("Description");
                _console.WriteLine(Target.Description);
            }

            // Option details
            _console.WriteLine();
            _console.WriteTitle("Options");
            WriteOptionDetails();

            // TODO - examples
            // TODO - see also?

        }



        private void WriteOptionDetails()
        {
            bool first = true;
            foreach (var p in Target.NamedParameters)
            {
                List<string> opts = new List<string>();

                opts.AddRange(p.LongNames.Select(x => "--" + x));
                opts.AddRange(p.ShortNames.Select(x => "-" + x));

                if (first)
                {
                    first = false;
                }
                else
                {
                    _console.WriteLine();
                }

                _console.WriteLine(string.Join(", ", opts));
                if (p.Description != null)
                {
                    _console.StartIndent();
                    // TODO - better formatting of long descriptions
                    _console.Write(p.Description.Trim(Environment.NewLine.ToCharArray()));
                    _console.EndIndent();
                }
            }
        }



        private static string ExeName()
        {
            // TODO - allow the name to be overridden in settings

            var path = Environment.GetCommandLineArgs()[0];
            if (string.IsNullOrEmpty(path))
            {
                path = "Unknown";
            }

            return Path.GetFileNameWithoutExtension(path).ToLower();
        }
    }
}
