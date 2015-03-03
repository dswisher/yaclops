# TODO List #

### Core ###

* Allow aliases; see sample AddCommand: `-A`, `--all` and `--no-ignore-removal`
* Create default short name for options, as long as it does not conflict. For example, the `Name` option should get `-n` as a short name, unless another option explicitly sets `-n` or there is a conflict such as a `Network` option.
* Handle global options that do not require a command, for things like `--version` or `--help`.
* Add new `settings` parameter to `Parse()` that can alter the behavior of the parser
    * Disable colored console when emitting help 
    * Change delimiter from `-` to `/`
    * Make options opt-out instead of opt-in (any property not explicitly excluded will become an option). Unclear how to differentiate parameters from options; perhaps parameters are still opt-in?
* Handle additional collection types (IEnumerable<string>, List<int>, etc)
* For collections, create the collection if not yet set? Or is that the responsibility of the subcommand class itself?
* If there are conflicts with commands (two items specify `-n` as a short name), the parser should throw
* Support `--` to indicate everything else is a parameter and not an option
* Implement defaults for parameters and options (so help can pick them up)


### Help ###

* Add short description to commands, and emit it in the command-list help page
* For subcommands with subcommands (like `bisect` in sample), only display one line in command list page - **where to get the summary, though**?
* Add long description to commands, and emit (using Markdown) in the detailed help page for a command
* Add description to parameters and flags, and emit (using MD) in the detailed help page for a command
* Add ability to generate HTML help and pop it up, like `git` does
* Add cross-reference ability (mainly for html)
