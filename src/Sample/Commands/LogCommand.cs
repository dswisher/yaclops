using Sample.Helpers;
using Yaclops;

namespace Sample.Commands
{
    [Summary("Show commit logs")]
    public class LogCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
