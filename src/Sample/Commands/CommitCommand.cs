using Sample.Helpers;
using Yaclops;

namespace Sample.Commands
{
    [Summary("Record changes to the repository")]
    public class CommitCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
