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
                var globals = new GlobalSettings();

                var settings = new CommandLineParserSettings<ISubCommand>
                {
                    ProgramName = "ConMan"
                };

                GlobalParser parser = new GlobalParser(settings);
                parser.AddGlobalOptions(globals);

                if (parser.Parse(args))
                {
                    globals.Execute();
                }
            }
            catch (CommandLineParserException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }



    class GlobalSettings
    {
        public GlobalSettings()
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
