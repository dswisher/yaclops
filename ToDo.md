# TODO List #

* Add description of commands, parameters and options
* Allow aliases; see sample AddCommand: `-A`, `--all` and `--no-ignore-removal`
* Create default short name for options, as long as it does not conflict. For example, the `Name` option should get `-n` as a short name, unless another option explicitly sets `-n` or there is a conflict such as a `Network` option.
* Handle global options that do not require a command, for things like `--version` or `--help`.
* Add new `settings` parameter to `Parse()` that can alter the behavior of the parser
    * Change delimiter from `-` to `/`
    * Make options opt-out instead of opt-in (any property not explicitly excluded will become an option). Unclear how to differentiate parameters from options; perhaps parameters are still opt-in?
* Handle additional collection types (IEnumerable<string>, List<int>, etc)
* For collections, create the collection if not yet set? Or is that the responsibility of the subcommand class itself?
* If there are conflicts with commands (two items specify `-n` as a short name), the parser should throw
* Support `--` to indicate everything else is a parameter and not an option
* Implement defaults
* Improve help
