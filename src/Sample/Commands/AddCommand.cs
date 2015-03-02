using System.Collections;
using System.Collections.Generic;
using Sample.Helpers;
using Yaclops;

namespace Sample.Commands
{
    public class AddCommand : ISubCommand
    {
        public AddCommand()
        {
            // TODO - shouldn't library create list if it does not exist?
            Paths = new List<string>();
        }


        [CommandLineParameter]
        public List<string> Paths { get; private set; }

        [CommandLineOption(ShortName="n")]
        public bool DryRun { get; set; }

        [CommandLineOption(ShortName = "v")]
        public bool Verbose { get; set; }

        [CommandLineOption(ShortName = "f")]
        public bool Force { get; set; }

        [CommandLineOption(ShortName = "i")]
        public bool Interactive { get; set; }

        [CommandLineOption(ShortName = "p")]
        public bool Patch { get; set; }

        [CommandLineOption(ShortName = "u")]
        public bool Update { get; set; }

        [CommandLineOption(ShortName = "A")]    // TODO: support --no-ignore-removal
        public bool All { get; set; }

        [CommandLineOption]    // TODO: support --ignore-removal
        public bool NoAll { get; set; }

        [CommandLineOption(ShortName = "N")]
        public bool IntentToAdd { get; set; }

        [CommandLineOption]
        public bool Refresh { get; set; }

        [CommandLineOption]
        public bool IgnoreErrors { get; set; }

        [CommandLineOption]
        public bool IgnoreMissing { get; set; }


        // TODO - add remaining parameters

        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
