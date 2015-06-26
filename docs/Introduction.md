# Introduction to Yaclops #

Conceptually, there are two steps to using Yaclops:

1. Configuring the parser
2. Parsing a command line (and optionally executing the command)

Each of these is discussed in a section below.


## Configuring the Parser ##

TBD


## Parsing a Command Line ##

To parse a command line, call one of the `Parse` overloads on the `CommandLineParser` class:

	public ISubCommand Parse(IEnumerable<string> input)
	public ISubCommand Parse(string input)

These are just specific cases of the more general `CommandLineParser<T>` class:

	public T Parse(IEnumerable<string> input)
	public T Parse(string input)

