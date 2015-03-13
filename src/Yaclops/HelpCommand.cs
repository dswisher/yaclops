using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonMark;
using CommonMark.Syntax;
using Yaclops.Help;
using Yaclops.Markdown;


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

                var ast = CreateDetail(command);

                AstPrinter.WriteAst(Console.Out, ast);

                ConsolePrinter.WriteAst(ast);
            }
            else
            {
                var document = HelpBuilder.CommandList(_parser.Commands);

                var target = new WrappedConsole();
                var renderer = new ConsoleRenderer(target);

                renderer.Render(document);
            }
        }



        private Block CreateDetail(ISubCommand command)
        {
            // TODO - for now, build up the markdown and parse it, rather than building the AST directly

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("# Name #");
            builder.AppendLine(command.Name());

            builder.AppendLine("# Synopsis #");
            builder.AppendLine("TBD.");

            builder.AppendLine("# Description #");
            if (command.Description() == null)
            {
                builder.AppendLine("n/a");
            }
            else
            {
                builder.AppendLine(command.Description());
            }

            var settings = CommonMarkSettings.Default.Clone();
            settings.OutputFormat = OutputFormat.SyntaxTree;

            var result = CommonMarkConverter.Parse(builder.ToString(), settings);

            return result;
        }
    }
}
