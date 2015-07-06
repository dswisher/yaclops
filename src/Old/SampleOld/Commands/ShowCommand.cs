using Sample.Helpers;
using Yaclops;
using Yaclops.Attributes;

namespace Sample.Commands
{
    [Summary("Show various types of objects")]
    public class ShowCommand : ISubCommand
    {
        [NamedParameter]
        public string Format { get; set; }


        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
