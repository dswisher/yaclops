using Sample.Helpers;
using Yaclops;

namespace Sample.Commands
{
    [Summary("Move or rename a file, a directory, or a symlink")]
    public class MvCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
