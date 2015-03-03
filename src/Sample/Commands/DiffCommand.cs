using Sample.Helpers;
using Yaclops;

namespace Sample.Commands
{
    [Summary("Show changes between commits, commit and working tree, etc")]
    public class DiffCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
