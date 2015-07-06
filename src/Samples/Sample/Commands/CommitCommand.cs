using System.ComponentModel;
using Yaclops.Attributes;

namespace Sample.Commands
{
    [Summary("Record changes to the repository")]
    [Description(@"
Stores the current contents of the index in a new commit along
with a log message from the user describing the changes.

The content to be added can be specified in several ways:

1. by using 'git add' to incrementally ""add"" changes to the
   index before using the 'commit' command (Note: even modified
   files must be ""added"");

2. by using 'git rm' to remove files from the working tree
   and the index, again before using the 'commit' command;

3. by listing files as arguments to the 'commit' command, in which
   case the commit will ignore changes staged in the index, and instead
   record the current content of the listed files (which must already
   be known to Git);

4. by using the -a switch with the 'commit' command to automatically
   ""add"" changes from all known files (i.e. all files that are already
   listed in the index) and to automatically ""rm"" files in the index
   that have been removed from the working tree, and then perform the
   actual commit;

5. by using the --interactive or --patch switches with the 'commit' command
   to decide one by one which files or hunks should be part of the commit,
   before finalizing the operation. See the ``Interactive Mode'' section of
   linkgit:git-add[1] to learn how to operate these modes.

The `--dry-run` option can be used to obtain a
summary of what is included by any of the above for the next
commit by giving the same set of parameters (options and paths).

If you make a commit and then find a mistake immediately after
that, you can recover from it with 'git reset'.
")]
    public class CommitCommand : ISampleCommand
    {
        [NamedParameter, ShortName("m")]
        [Description("Use the given <msg> as the commit message. If multiple `-m` options are given, their values are concatenated as separate paragraphs.")]
        public string Message { get; set; }
    }
}
