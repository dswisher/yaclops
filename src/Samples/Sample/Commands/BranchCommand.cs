using System.ComponentModel;
using Yaclops.Attributes;

namespace Sample.Commands
{
    [Summary("List, create, or delete branches")]
    public class BranchCommand : ISampleCommand
    {
        [NamedParameter, ShortName("d")]
        // TODO - better handling of long descriptions
        //[Description("Delete a branch. The branch must be fully merged in its upstream branch, or in HEAD if no upstream was set with --track or --set-upstream.")]
        [Description("Delete a branch. The branch must be fully merged...")]
        public bool Delete { get; set; }

        [NamedParameter, ShortName("D")]
        [Description("Delete a branch irrespective of its merged status.")]
        public bool ForceDelete { get; set; }

        [PositionalParameter, Mandatory]
        [Description("The name of the branch")]
        public string NewBranch { get; set; }
    }
}
