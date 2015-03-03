using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonMark;
using CommonMark.Syntax;
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

                Console.WriteLine("Detailed help on '{0}' is not yet available...", command.Name());
            }
            else
            {
                var ast = CreateCommandList();

                // TODO - dump the AST for debugging
                // AstPrinter.WriteAst(Console.Out, ast);

                ConsolePrinter.WriteAst(ast);
            }
        }



        private Block CreateCommandList()
        {
            // TODO - for now, build up the markdown and parse it, rather than building the AST directly

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("# Commands #");

            foreach (var command in _parser.Commands.OrderBy(x => x.Name()))
            {
                builder.AppendLine(string.Concat("* `", command.Name(), "` ", command.Summary()));
            }

            var settings = new CommonMarkSettings
            {
                OutputFormat = OutputFormat.SyntaxTree
            };

            var result = CommonMarkConverter.Parse(builder.ToString(), settings);

            return result;
        }
    }
}
