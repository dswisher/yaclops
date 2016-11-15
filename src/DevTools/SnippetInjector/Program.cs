using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using SampleHelpers;
using Yaclops;
using Yaclops.Attributes;

namespace SnippetInjector
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var globals = new GlobalSettings();

                var parser = new GlobalParser();
                parser.AddGlobalOptions(globals);

                if (parser.Parse(args))
                {
                    if (!globals.NoLogo)
                    {
                        PrintLogo();
                    }

                    // TODO - need to handle positional parameters at the group (global) level!
                    Console.WriteLine("** Inject Snippets **");
                    globals.Dump();
                }
            }
            catch (CommandLineParserException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception in main:");
                Console.WriteLine(ex);
            }

            if (Debugger.IsAttached)
            {
                Console.Write("<press ENTER to continue>");
                Console.ReadLine();
            }
        }



        public static void PrintLogo()
        {
            const string logo = @"
  _____        _           _             
  \_   \_ __  (_) ___  ___| |_ ___  _ __ 
   / /\/ '_ \ | |/ _ \/ __| __/ _ \| '__|
/\/ /_ | | | || |  __/ (__| || (_) | |   
\____/ |_| |_|/ |\___|\___|\__\___/|_|   
            |__/                         
";

            Console.WriteLine(logo);
        }



        private class GlobalSettings
        {
            [PositionalParameter, Mandatory]
            [Description("The list of file names to update. Wildcards are allowed.")]
            public List<string> Names { get; set; }

            [NamedParameter, ShortName("n")]
            public bool NoLogo { get; set; }
        }
    }
}
