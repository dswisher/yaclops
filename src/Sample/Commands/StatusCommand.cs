using Sample.Helpers;
using Yaclops;
using Yaclops.Attributes;

namespace Sample.Commands
{
    [Summary("Show the working tree status")]
    public class StatusCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
