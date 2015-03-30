using System.Collections.Generic;
using System.ComponentModel;
using Sample.Helpers;
using Yaclops;
using Yaclops.Attributes;

namespace Sample.Commands
{
    [Summary("Add file contents to the index")]
    [Description(@"
This command updates the index using the current content found in the working tree,
to prepare the content staged for the next commit.
It typically adds the current content of existing paths as a whole, but with some options
it can also be used to add content with only part of the changes made to the working tree
files applied, or remove paths that do not exist in the working tree anymore.

The ""index"" holds a snapshot of the content of the working tree, and it is this snapshot
that is taken as the contents of the next commit.
Thus after making any changes to the working directory, and before running the commit
command, you must use the add command to add any new or modified files to the index.

This command can be performed multiple times before a commit. It only adds the content of
the specified file(s) at the time the add command is run; if you want subsequent changes
included in the next commit, then you must run git add again to add the new content to the
index.

The git status command can be used to obtain a summary of which files have changes that are
staged for the next commit.

The git add command will not add ignored files by default. If any ignored files were
explicitly specified on the command line, git add will fail with a list of ignored files.
Ignored files reached by directory recursion or filename globbing performed by Git (quote
your globs before the shell) will be silently ignored. The git add command can be used
to add ignored files with the -f (force) option.

Please see `commit` for alternative ways to add content to a commit.")]
    public class AddCommand : ISubCommand
    {
        public AddCommand()
        {
            // TODO - shouldn't library create list if it does not exist?
            Paths = new List<string>();
        }


        [PositionalParameter]
        public List<string> Paths { get; private set; }

        [NamedParameter(ShortName="n")]
        [Description("Don’t actually add the file(s), just show if they exist and/or will be ignored.")]
        public bool DryRun { get; set; }

        [NamedParameter(ShortName = "v")]
        [Description("Be verbose.")]
        public bool Verbose { get; set; }

        [NamedParameter(ShortName = "f")]
        [Description("Allow adding otherwise ignored files.")]
        public bool Force { get; set; }

        [NamedParameter(ShortName = "i")]
        [Description(@"
Add modified contents in the working tree interactively to the index.
Optional path arguments may be supplied to limit operation to a subset of the working tree.
See ""Interactive mode"" for details.
")]
        public bool Interactive { get; set; }

        [NamedParameter(ShortName = "p")]
        [Description(@"
Interactively choose hunks of patch between the index and the work tree and add them to the index.
This gives the user a chance to review the difference before adding modified contents to the index.

This effectively runs add --interactive, but bypasses the initial command menu and directly jumps to the patch subcommand.
See ""Interactive mode"" for details.
")]
        public bool Patch { get; set; }

        [NamedParameter(ShortName = "e")]
        [Description(@"
Open the diff vs. the index in an editor and let the user edit it.
After the editor was closed, adjust the hunk headers and apply the patch to the index.

The intent of this option is to pick and choose lines of the patch to apply, or even to modify the contents of lines to be staged.
This can be quicker and more flexible than using the interactive hunk selector.
However, it is easy to confuse oneself and create a patch that does not apply to the index.
See EDITING PATCHES below.
")]
        public bool Edit { get; set; }

        [NamedParameter(ShortName = "u")]
        public bool Update { get; set; }

        [NamedParameter(ShortName = "A")]    // TODO: support --no-ignore-removal
        public bool All { get; set; }

        [NamedParameter]    // TODO: support --ignore-removal
        public bool NoAll { get; set; }

        [NamedParameter(ShortName = "N")]
        public bool IntentToAdd { get; set; }

        [NamedParameter]
        public bool Refresh { get; set; }

        [NamedParameter]
        public bool IgnoreErrors { get; set; }

        [NamedParameter]
        public bool IgnoreMissing { get; set; }


        // TODO - add remaining parameters

        public void Execute()
        {
            // Execute the command. For demo purposes, just dump out the parameters...
            this.Dump();
        }
    }
}
