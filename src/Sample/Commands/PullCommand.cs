using Sample.Helpers;
using Yaclops;
using Yaclops.Attributes;

namespace Sample.Commands
{
    [Summary("Fetch from and integrate with another repository or a local branch")]
    public class PullCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
