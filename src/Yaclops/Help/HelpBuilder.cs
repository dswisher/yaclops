using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Yaclops.Help
{
    internal static class HelpBuilder
    {
        public static void WriteCommandList(IEnumerable<ISubCommand> commands, IConsole console)
        {
            List<CommandEntry> entries = new List<CommandEntry>();
            HashSet<string> names = new HashSet<string>();

            foreach (var command in commands)
            {
                bool multi = false;
                string name = command.Name();
                if (name.Contains(' '))
                {
                    name = name.Substring(0, name.IndexOf(' '));
                    multi = true;
                }

                if (names.Contains(name))
                {
                    continue;
                }

                names.Add(name);

                var entry = new CommandEntry
                {
                    Name = name,
                };

                if (multi)
                {
                    // TODO - if this is a multi-word command (like bisect), find a generic summary for it
                    entry.Summary = "TBD";
                }
                else
                {
                    entry.Summary = command.Summary();
                }

                entries.Add(entry);
            }

            int maxLength = entries.Select(x => x.Name.Length).Max();

            foreach (var command in entries.OrderBy(x => x.Name))
            {
                console.WriteLine("   {0}   {1}", command.Name.PadRight(maxLength), command.Summary);
            }
        }



        public static string ExeName()
        {
            // TODO - allow the name to be overridden in settings
            return Assembly.GetEntryAssembly().GetName().Name.ToLower();
        }



        public static void WriteSynopsis(ISubCommand command, IConsole console)
        {
            console.StartWrap("{0} {1}", ExeName(), command.Name());

            // TODO - repeat global flags?

            // Write out the named parameters
            foreach (var p in command.NamedParameters())
            {
                List<string> opts = new List<string>();

                if (!string.IsNullOrEmpty(p.Attribute.LongName))
                {
                    opts.Add("--" + p.Attribute.LongName);
                }
                else
                {
                    opts.Add("--" + p.Property.Name.Decamel('-'));
                }

                if (!string.IsNullOrEmpty(p.Attribute.ShortName))
                {
                    opts.Add("-" + p.Attribute.ShortName);
                }

                console.Write(" [{0}]", string.Join(" | ", opts));
            }

            // Write out the positional parameters
            foreach (var p in command.PositionalParameters())
            {
                // TODO - for lists, indicate mmultiple
                console.Write(" [<{0}>]", p.Property.Name.Decamel());
            }

            // Finish off the synopsis
            console.EndWrap();
        }



        public static void WriteOptionDetails(ISubCommand command, IConsole console)
        {
            // TODO
            console.WriteLine("TBD");
        }



        private class CommandEntry
        {
            public string Name { get; set; }
            public string Summary { get; set; }
        }
    }
}
