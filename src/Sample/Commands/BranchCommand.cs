using Sample.Helpers;
using Yaclops;

namespace Sample.Commands
{
    [Summary("List, create, or delete branches")]
    public class BranchCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
