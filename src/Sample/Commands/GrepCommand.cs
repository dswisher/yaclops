using Sample.Helpers;
using Yaclops;

namespace Sample.Commands
{
    public class GrepCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
