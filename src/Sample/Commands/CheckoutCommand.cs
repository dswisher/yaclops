using Sample.Helpers;
using Yaclops;
using Yaclops.Attributes;

namespace Sample.Commands
{
    [Summary("Checkout a branch or paths to the working tree")]
    public class CheckoutCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
