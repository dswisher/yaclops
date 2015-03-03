using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonMark;
using CommonMark.Syntax;


namespace Yaclops
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
            if (Commands.Any())
            {
                ISubCommand command;
                try
                {
                    command = _parser.Parse(Commands);
                }
                catch (CommandLineParserException)
                {
                    Console.WriteLine("Information on that command is not available, as it does not exist.");
                    return;
                }

                Console.WriteLine("Detailed help on '{0}' is not yet available...", command.Name());
            }
            else
            {
                var ast = CreateCommandList();

                WriteAstToConsole(ast);

                //Console.WriteLine("Commands:");

                //foreach (var command in _parser.Commands.OrderBy(x => x.Name()))
                //{
                //    Console.WriteLine("   {0}", command.Name());
                //}
            }
        }



        private Block CreateCommandList()
        {
            // TODO - for now, build up the markdown and parse it, rather than building the AST directly

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("# Commands #");

            foreach (var command in _parser.Commands.OrderBy(x => x.Name()))
            {
                // Console.WriteLine("   {0}", command.Name());
                builder.AppendLine(string.Join("- `", command.Name(), "` ", command.Summary()));
            }

            var settings = new CommonMarkSettings
            {
                OutputFormat = OutputFormat.SyntaxTree // for now
            };

            var result = CommonMarkConverter.Parse(builder.ToString(), settings);

            return result;
        }



        private void WriteAstToConsole(Block block)
        {
            // TODO - for now, just dump the AST; cobbled from Printer.PrintBlocks

            int indent = 0;
            var stack = new Stack<BlockStackEntry>();

            while (block != null)
            {
                Console.Write(new string(' ', indent));

                switch (block.Tag)
                {
                    case BlockTag.Document:
                        Console.Write("document");
                        break;

                    // TODO - handle remaining block types
#if false
                    case BlockTag.BlockQuote:
                        writer.Write("block_quote");
                        PrintPosition(trackPositions, writer, block);
                        break;

                    case BlockTag.ListItem:
                        writer.Write("list_item");
                        PrintPosition(trackPositions, writer, block);
                        break;

                    case BlockTag.List:
                        writer.Write("list");
                        PrintPosition(trackPositions, writer, block);

                        var data = block.ListData;
                        if (data.ListType == ListType.Ordered)
                        {
                            writer.Write(" (type=ordered tight={0} start={1} delim={2})",
                                 data.IsTight,
                                 data.Start,
                                 data.Delimiter);
                        }
                        else
                        {
                            writer.Write("(type=bullet tight={0} bullet_char={1})",
                                 data.IsTight,
                                 data.BulletChar);
                        }
                        break;
#endif

                    case BlockTag.AtxHeader:
                        Console.Write("atx_header");
                        Console.Write(" (level={0})", block.HeaderLevel);
                        break;

#if false
                    case BlockTag.SETextHeader:
                        writer.Write("setext_header");
                        PrintPosition(trackPositions, writer, block);
                        writer.Write(" (level={0})", block.HeaderLevel);
                        break;
#endif

                    case BlockTag.Paragraph:
                        Console.Write("paragraph");
                        break;

#if false
                    case BlockTag.HorizontalRuler:
                        writer.Write("hrule");
                        PrintPosition(trackPositions, writer, block);
                        break;

                    case BlockTag.IndentedCode:
                        writer.Write("indented_code {0}", format_str(block.StringContent.ToString(buffer), buffer));
                        PrintPosition(trackPositions, writer, block);
                        writer.Write(' ');
                        writer.Write(format_str(block.StringContent.ToString(buffer), buffer));
                        break;

                    case BlockTag.FencedCode:
                        writer.Write("fenced_code");
                        PrintPosition(trackPositions, writer, block);
                        writer.Write(" length={0} info={1} {2}",
                               block.FencedCodeData.FenceLength,
                               format_str(block.FencedCodeData.Info, buffer),
                               format_str(block.StringContent.ToString(buffer), buffer));
                        break;

                    case BlockTag.HtmlBlock:
                        writer.Write("html_block");
                        PrintPosition(trackPositions, writer, block);
                        writer.Write(' ');
                        writer.Write(format_str(block.StringContent.ToString(buffer), buffer));
                        break;

                    case BlockTag.ReferenceDefinition:
                        writer.Write("reference_def");
                        PrintPosition(trackPositions, writer, block);
                        break;
#endif

                    default:
                        throw new NotImplementedException("Block " + block.Tag + " is not yet handled!");
                }

                Console.WriteLine();

                // TODO - handle inline context
#if false
                if (block.InlineContent != null)
                {
                    PrintInlines(writer, block.InlineContent, indent + 2, inlineStack, buffer, trackPositions);
                }

#endif

                if (block.FirstChild != null)
                {
                    if (block.NextSibling != null)
                        stack.Push(new BlockStackEntry(indent, block.NextSibling));

                    indent += 2;
                    block = block.FirstChild;
                }
                else if (block.NextSibling != null)
                {
                    block = block.NextSibling;
                }
                else if (stack.Count > 0)
                {
                    var entry = stack.Pop();
                    indent = entry.Indent;
                    block = entry.Target;
                }
                else
                {
                    block = null;
                }
            }
        }



#if false
        private static void PrintInlines(System.IO.TextWriter writer, Inline inline, int indent, Stack<InlineStackEntry> stack, StringBuilder buffer, bool trackPositions)
        {
            while (inline != null)
            {
                writer.Write(new string(' ', indent));

                switch (inline.Tag)
                {
                    case InlineTag.String:
                        writer.Write("str");
                        PrintPosition(trackPositions, writer, inline);
                        writer.Write(' ');
                        writer.Write(format_str(inline.LiteralContent, buffer));
                        break;

                    case InlineTag.LineBreak:
                        writer.Write("linebreak");
                        PrintPosition(trackPositions, writer, inline);
                        break;

                    case InlineTag.SoftBreak:
                        writer.Write("softbreak");
                        PrintPosition(trackPositions, writer, inline);
                        break;

                    case InlineTag.Code:
                        writer.Write("code {0}", format_str(inline.LiteralContent, buffer));
                        PrintPosition(trackPositions, writer, inline);
                        writer.Write(' ');
                        writer.Write(format_str(inline.LiteralContent, buffer));
                        break;

                    case InlineTag.RawHtml:
                        writer.Write("html {0}", format_str(inline.LiteralContent, buffer));
                        writer.Write(' ');
                        writer.Write(format_str(inline.LiteralContent, buffer));
                        break;

                    case InlineTag.Link:
                        writer.Write("link");
                        PrintPosition(trackPositions, writer, inline);
                        writer.Write(" url={0} title={1}",
                               format_str(inline.TargetUrl, buffer),
                               format_str(inline.LiteralContent, buffer));
                        break;

                    case InlineTag.Image:
                        writer.Write("image");
                        PrintPosition(trackPositions, writer, inline);
                        writer.Write(" url={0} title={1}",
                               format_str(inline.TargetUrl, buffer),
                               format_str(inline.LiteralContent, buffer));
                        break;

                    case InlineTag.Strong:
                        writer.Write("strong");
                        PrintPosition(trackPositions, writer, inline);
                        break;

                    case InlineTag.Emphasis:
                        writer.Write("emph");
                        PrintPosition(trackPositions, writer, inline);
                        break;

                    case InlineTag.Strikethrough:
                        writer.Write("del");
                        PrintPosition(trackPositions, writer, inline);
                        break;

                    default:
                        writer.Write("unknown: " + inline.Tag.ToString());
                        PrintPosition(trackPositions, writer, inline);
                        break;
                }

                writer.WriteLine();

                if (inline.FirstChild != null)
                {
                    if (inline.NextSibling != null)
                        stack.Push(new InlineStackEntry(indent, inline.NextSibling));

                    indent += 2;
                    inline = inline.FirstChild;
                }
                else if (inline.NextSibling != null)
                {
                    inline = inline.NextSibling;
                }
                else if (stack.Count > 0)
                {
                    var entry = stack.Pop();
                    indent = entry.Indent;
                    inline = entry.Target;
                }
                else
                {
                    inline = null;
                }
            }
        }
#endif

        private struct BlockStackEntry
        {
            public readonly int Indent;
            public readonly Block Target;
            public BlockStackEntry(int indent, Block target)
            {
                this.Indent = indent;
                this.Target = target;
            }
        }
        private struct InlineStackEntry
        {
            public readonly int Indent;
            public readonly Inline Target;
            public InlineStackEntry(int indent, Inline target)
            {
                this.Indent = indent;
                this.Target = target;
            }
        }
    }
}
