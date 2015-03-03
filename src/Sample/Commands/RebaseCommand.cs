using Sample.Helpers;
using Yaclops;

namespace Sample.Commands
{
    [Summary("Forward-port local commits to the updated upstream head")]
    public class RebaseCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
