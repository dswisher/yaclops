using Sample.Helpers;
using Yaclops;
using Yaclops.Attributes;

namespace Sample.Commands
{
    [Summary("Remove files from the working tree and from the index")]
    public class RmCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
