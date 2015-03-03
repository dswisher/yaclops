using Sample.Helpers;
using Yaclops;

namespace Sample.Commands
{
    [Summary("Update remote refs along with associated objects")]
    public class PushCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
