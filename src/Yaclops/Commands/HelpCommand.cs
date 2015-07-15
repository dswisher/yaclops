using System;
using System.Collections.Generic;
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

            var para = doc.AddParagraph(new Paragraph(new ParagraphStyle { LinesAfter = 1 }));
            para.AddSpan("usage: " + VerbPath(group));

            AddNamed(para, group.NamedParameters);
            // TODO - include --help!

            if (group.Nodes.Any())
            {
                para.AddSpan("<command>");
            }

            if (group.Nodes.Any(x => x.PositionalParameters.Any()))
            {
                para.AddSpan("[<args>]");
            }

            // TODO - add usage
            // TODO - add overall command description

            // TODO - include "Help" in the command list!
            AddCommandList(doc, group);

            // TODO - add help blurb
            if (group.Nodes.Any())
            {
                var p = doc.AddParagraph(new Paragraph(new ParagraphStyle { LinesBefore = 1 }));
                var helpVerb = "help";  // TODO - pull from settings!
                p.AddSpan("See '{0} {1} <command>' to read about a specific subcommand.", VerbPath(group), helpVerb);
            }

            return doc;
        }



        private void AddNamed(Paragraph para, IList<CommandNamedParameter> parameters)
        {
            foreach (var p in parameters)
            {
                // TODO - set the spans to not split whitespace
                para.AddSpan(p.Usage);
            }
        }



        private void AddCommandList(Document doc, CommandGroup group)
        {
            ParagraphStyle style = new ParagraphStyle
            {
                Indent = 3,
                Tabs = new[] { 22 }
            };

            // TODO - add option to show ALL commands/groups
            const bool showHidden = false;

            var commands = GetExpandedCommandNodes(group, showHidden)
                .Select(x => new { x.Node, x.Depth, Verb = VerbPath(x.Node, x.Depth - 1) });

            foreach (var entry in commands.OrderBy(x => x.Verb))
            {
                if (entry.Node.Hidden)
                {
                    continue;
                }

                var para = doc.AddParagraph(new Paragraph(style));

                // TODO - highlight the command by setting span style?
                para.AddSpan(new Span(entry.Verb));
                para.AddTab();

                if (!string.IsNullOrEmpty(entry.Node.Summary))
                {
                    para.AddSpan(new Span(entry.Node.Summary));
                }
            }
        }



        private IEnumerable<CommandChildEntry> GetExpandedCommandNodes(CommandGroup group, bool showHidden)
        {
            // Can't use recursion here, as that prevents yielding the results, so use a stack...
            Stack<CommandChildEntry> stack = new Stack<CommandChildEntry>(new[] { new CommandChildEntry(group, 0) });

            while (stack.Count > 0)
            {
                var entry = stack.Pop();

                var command = entry.Node as Command;
                if ((command != null) && (showHidden || !command.Hidden))
                {
                    yield return entry;
                }

                var childGroup = entry.Node as CommandGroup;
                // TODO - the '3' is arbitrary - make it configurable! (Determines when to show children)
                if ((childGroup != null) && ((entry.Depth == 0) || (CountChildCommands(childGroup, showHidden) <= 3)))
                {
                    foreach (var child in childGroup.Nodes)
                    {
                        if (showHidden || !child.Hidden)
                        {
                            stack.Push(new CommandChildEntry(child, entry.Depth + 1));
                        }
                    }
                }
            }
        }



        private int CountChildCommands(CommandNode node, bool includeHidden)
        {
            int count = 0;

            Command command = node as Command;
            if (command != null)
            {
                count += 1;
            }
            else
            {
                CommandGroup group = node as CommandGroup;
                if ((group != null) && (includeHidden || !group.Hidden))
                {
                    count += group.Nodes.Sum(child => CountChildCommands(child, includeHidden));
                }
            }

            return count;
        }



        private Document CommandHelp(Command command)
        {
            Document doc = new Document();

            // TODO - create a "style sheet" that can be passed around
            var headerStyle = new ParagraphStyle { LinesBefore = 1 };

            AddName(command, doc, headerStyle);
            AddSynopsis(command, doc, headerStyle);

            // TODO - add long description

            AddOptions(command, doc, headerStyle);

            return doc;
        }



        private void AddName(Command command, Document doc, ParagraphStyle headerStyle)
        {
            var para = doc.AddParagraph(new Paragraph(headerStyle));
            para.AddSpan("NAME");
            // TODO - rather than drawing dashes, make the prior span a different color or some such
            para = doc.AddParagraph(new Paragraph());
            para.AddSpan("----");

            para = doc.AddParagraph(new Paragraph());
            para.AddSpan(VerbPath(command));
            para.AddSpan("-");
            para.AddSpan(command.Summary);
        }



        private void AddOptions(Command command, Document doc, ParagraphStyle headerStyle)
        {
            var para = doc.AddParagraph(new Paragraph(headerStyle));
            para.AddSpan("OPTIONS");
            para = doc.AddParagraph(new Paragraph());
            para.AddSpan("-------");

            var descStyle = new ParagraphStyle
            {
                Indent = 3,
                LinesAfter = 1
            };

            foreach (var named in command.NamedParameters)
            {
                para = doc.AddParagraph(new Paragraph());
                para.AddSpan(named.Usage);

                foreach (var p in MarkLeft.Parse(named.Description, descStyle))
                {
                    doc.AddParagraph(p);
                }
            }

            // TODO - add positional parameters from prior verbs
            foreach (var pos in command.PositionalParameters)
            {
                para = doc.AddParagraph(new Paragraph());
                para.AddSpan(pos.Usage);

                foreach (var p in MarkLeft.Parse(pos.Description, descStyle))
                {
                    doc.AddParagraph(p);
                }
            }
        }



        private void AddSynopsis(Command command, Document doc, ParagraphStyle headerStyle)
        {
            var para = doc.AddParagraph(new Paragraph(headerStyle));
            para.AddSpan("SYNOPSIS");
            para = doc.AddParagraph(new Paragraph());
            para.AddSpan("--------");

            para = doc.AddParagraph(new Paragraph());
            para.AddSpan(VerbPath(command));

            foreach (var named in command.NamedParameters)
            {
                para.AddSpan(named.Usage);
            }

            // TODO - pick up any positional parameters on prior verbs
            foreach (var pos in command.PositionalParameters)
            {
                para.AddSpan(pos.Usage);
            }
        }



        // TODO - include options of ancestor groups?
        private string VerbPath(CommandNode node, int maxDepth = int.MaxValue)
        {
            StringBuilder builder = new StringBuilder();

            if ((node.Parent != null) && (maxDepth > 0))
            {
                builder.Append(VerbPath(node.Parent, maxDepth - 1));
            }

            if (builder.Length > 0)
            {
                builder.Append(" ");
            }

            builder.Append(node.HelpVerb);

            return builder.ToString();
        }



        private class CommandChildEntry
        {
            public CommandChildEntry(CommandNode node, int depth)
            {
                Node = node;
                Depth = depth;
            }

            public CommandNode Node { get; private set; }
            public int Depth { get; private set; }
        }
    }
}
