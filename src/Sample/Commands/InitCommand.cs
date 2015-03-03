using Sample.Helpers;
using Yaclops;

namespace Sample.Commands
{
    [Summary("Create an empty Git repository or reinitialize an existing one")]
    public class InitCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
