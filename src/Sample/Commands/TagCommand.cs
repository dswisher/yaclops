using Sample.Helpers;
using Yaclops;
using Yaclops.Attributes;

namespace Sample.Commands
{
    [Summary("Create, list, delete or verify a tag object signed with GPG")]
    public class TagCommand : ISubCommand
    {
        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
