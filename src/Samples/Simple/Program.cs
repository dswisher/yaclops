using System;
using System.ComponentModel;
using SampleHelpers;
using Yaclops;
using Yaclops.Attributes;

namespace Simple
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var sampleCommand = new SimpleCommand();

                var settings = new CommandLineParserSettings<ISubCommand>
                {
                    ProgramName = "ConMan",
                    DefaultCommand = () => sampleCommand
                };

                CommandLineParser<ISubCommand> parser = new CommandLineParser(settings);
                parser.AddCommand(sampleCommand);

                var command = parser.Parse(args);

                command.Execute();
            }
            catch (CommandLineParserException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }



    class SimpleCommand : ISubCommand
    {
        public SimpleCommand()
        {
            Sample = "one";
            Rows = Console.WindowHeight;
            Columns = Console.WindowWidth;
        }


        [PositionalParameter]
        [Description("The name of the sample to run.")]
        public string Sample { get; set; }


        [NamedParameter, ShortName("r")]
        public int Rows { get; set; }


        [NamedParameter, ShortName("c")]
        public int Columns { get; set; }


        public void Execute()
        {
            this.Dump();
        }
    }
}
