using System;
using Yaclops.DocumentModel;
using Yaclops.Formatting;
using Yaclops.Model;

namespace Yaclops.Commands
{
    internal class HelpCommand
    {
        private readonly CommandNode _start;

        public static HelpCommand Make(CommandNode start)
        {
            return new HelpCommand(start);
        }


        public HelpCommand(CommandNode start)
        {
            _start = start;
        }


        public void Execute()
        {
            Console.WriteLine("Help is not yet implemented! Node.verb={0}, type={1}", _start.Verb, _start.GetType().Name);

            Document doc;
            var group = _start as CommandGroup;
            if (group != null)
            {
                doc = GroupHelp(group);
            }
            else
            {
                var command = _start as Command;
                if (command != null)
                {
                    doc = CommandHelp(command);
                }
                else
                {
                    throw new Exception("Unhandled case within internal help command!");
                }
            }

            ConsoleFormatter formatter = new ConsoleFormatter();
            formatter.Format(doc);
        }



        private Document GroupHelp(CommandGroup group)
        {
            Document doc = new Document();

            foreach (var node in group.Nodes)
            {
                var para = doc.AddParagraph(new Paragraph());
                // TODO - add formatting!
                // TODO - add summary!
                // TODO - allow nodes to be hidden, unless an 'all' option is set
                para.AddSpan(new Span(node.Verb));
            }

            return doc;
        }



        private Document CommandHelp(Command command)
        {
            Console.WriteLine("   --> Command.");
            // TODO
            return new Document();
        }
    }
}
