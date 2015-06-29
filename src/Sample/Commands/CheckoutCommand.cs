using System.ComponentModel;
using Sample.Helpers;
using Yaclops;
using Yaclops.Attributes;

namespace Sample.Commands
{
    [Summary("Checkout a branch or paths to the working tree")]
    public class CheckoutCommand : ISubCommand
    {
        [PositionalParameter]
        [Description(@"
Branch to checkout; if it refers to a branch (i.e., a name that, when prepended with ""refs/heads/"",
is a valid ref), then that branch is checked out. Otherwise, if it refers to a valid commit, your HEAD
becomes ""detached"" and you are no longer on any branch (see below for details).
")]
        public string Branch { get; set; }


        [NamedParameter, ShortName("q")]
        [Description(@"Quiet, suppress feedback messages.")]
        public bool Quiet { get; set; }


        [NamedParameter, ShortName("f")]
        [Description(@"
When switching branches, proceed even if the index or the working tree differs from HEAD.
This is used to throw away local changes.

When checking out paths from the index, do not fail upon unmerged entries; instead, unmerged
entries are ignored.
")]
        public bool Force { get; set; }


        // TODO - figure out how to handle variants: "git checkout [-p|--patch] [<tree-ish>] [--] <pathspec>…" versus "git checkout <branch>", etc...
        // TODO - add remaining parameters


        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
