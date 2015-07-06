using System;
using System.Linq;
using System.Text;
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

            var para = doc.AddParagraph(new Paragraph());
            para.AddSpan("usage: " + VerbPath(group));

            // TODO - add usage
            // TODO - add overall command description

            AddCommandList(doc, group);

            return doc;
        }



        private void AddCommandList(Document doc, CommandGroup group)
        {
            ParagraphStyle style = new ParagraphStyle
            {
                Indent = 3,
                Tabs = new[] { 22 }
            };

            foreach (var node in group.Nodes.OrderBy(x => x.Verb))
            {
                // TODO - add option to show ALL commands/groups
                if (node.Hidden)
                {
                    continue;
                }

                var para = doc.AddParagraph(new Paragraph(style));

                para.AddSpan(new Span(node.Verb));      // TODO - highlight the command?
                para.AddTab();

                if (!string.IsNullOrEmpty(node.Summary))
                {
                    para.AddSpan(new Span(node.Summary));
                }
            }
        }



        private Document CommandHelp(Command command)
        {
            Console.WriteLine("   --> Command.");
            // TODO
            return new Document();
        }



        // TODO - include options?
        private string VerbPath(CommandNode node)
        {
            StringBuilder builder = new StringBuilder();

            if (node.Parent != null)
            {
                builder.Append(VerbPath(node.Parent));
            }

            if (builder.Length > 0)
            {
                builder.Append(" ");
            }

            builder.Append(node.HelpVerb);

            return builder.ToString();
        }
    }
}
