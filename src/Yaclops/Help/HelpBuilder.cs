using System.Collections.Generic;
using System.Linq;


namespace Yaclops.Help
{
    internal static class HelpBuilder
    {

        public static ConsoleDocument CommandList(IEnumerable<ISubCommand> commands)
        {
            ConsoleDocument doc = new ConsoleDocument();

            doc.Add(new DocumentHeader("Commands"));

            var table = new DocumentTable();
            doc.Add(table);

            foreach (var command in commands.OrderBy(x => x.Name()))
            {
                var row = new DocumentTableRow();
                table.Add(row);

                row.Add(new DocumentTableColumn(command.Name()));
                row.Add(new DocumentTableColumn(command.Summary()));
            }

            return doc;
        }

    }
}
