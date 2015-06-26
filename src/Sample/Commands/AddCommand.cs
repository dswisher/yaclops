using Sample.Helpers;
using Yaclops;

namespace Sample.Commands
{
    public class AddCommand : ISubCommand
    {
        // TODO - add some options!

        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
