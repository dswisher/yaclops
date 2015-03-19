using System.Collections.Generic;
using System.Linq;


namespace Yaclops.Help
{
    internal static class HelpBuilder
    {
        public static void WriteCommandList(IEnumerable<ISubCommand> commands, IConsole console)
        {
            List<Entry> entries = new List<Entry>();
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

                var entry = new Entry
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

            int maxLength = entries.Select(x => x.Name.Length).Max(); ;

            foreach (var command in entries.OrderBy(x => x.Name))
            {
                console.WriteLine("   {0}   {1}", command.Name.PadRight(maxLength), command.Summary);
            }
        }



        private class Entry
        {
            public string Name { get; set; }
            public string Summary { get; set; }
        }
    }
}
