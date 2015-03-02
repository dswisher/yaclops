using Sample.Helpers;
using Yaclops;

namespace Sample.Commands
{
    public class AddCommand : ISubCommand
    {
        [CommandLineOption(ShortName="n")]
        public bool DryRun { get; set; }

        // TODO - add remaining parameters

        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
