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
            Document doc = new Document();

            var headerStyle = new ParagraphStyle { LinesBefore = 1 };

            var para = doc.AddParagraph(new Paragraph(headerStyle));
            para.AddSpan("NAME");
            // TODO - rather than drawing dashes, make the prior span a different color or some such
            para = doc.AddParagraph(new Paragraph());
            para.AddSpan("----");

            para = doc.AddParagraph(new Paragraph());
            para.AddSpan(VerbPath(command));
            para.AddSpan("-");
            para.AddSpan(command.Summary);

            para = doc.AddParagraph(new Paragraph(headerStyle));
            para.AddSpan("SYNOPSIS");
            para = doc.AddParagraph(new Paragraph());
            para.AddSpan("--------");

            para = doc.AddParagraph(new Paragraph());
            para.AddSpan(VerbPath(command));

            foreach (var named in command.NamedParameters)
            {
                para.AddSpan(named.Usage);
            }

            // TODO - pick up any position parameters on prior verbs
            foreach (var pos in command.PositionalParameters)
            {
                para.AddSpan(pos.Usage);
            }

            para = doc.AddParagraph(new Paragraph(headerStyle));
            para.AddSpan("OPTIONS");
            para = doc.AddParagraph(new Paragraph());
            para.AddSpan("-------");

            var firstStyle = new ParagraphStyle();

            var secondStyle = new ParagraphStyle
            {
                LinesBefore = 1
            };

            var descStyle = new ParagraphStyle
            {
                Indent = 3
            };

            bool first = true;
            foreach (var named in command.NamedParameters)
            {
                para = doc.AddParagraph(new Paragraph(first ? firstStyle : secondStyle));
                para.AddSpan(named.Usage);

                foreach (var p in MarkLeft.Parse(named.Description, descStyle))
                {
                    // TODO - style?
                    doc.AddParagraph(p);
                }

                // TODO - add named params to option list

                first = false;
            }

            // TODO - add positional parameters to option list

            // TODO - add long description

            return doc;
        }



        // TODO - include options of ancestor groups?
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
