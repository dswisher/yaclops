![](logo.png)

# Yaclops #

Yet another command-line option parser.

* Intended for cases where subcommands are required (like git or svn)
* Nearly POCO classes: they need to inherit an interface and implement one method (`Execute()`).
* Works well with IoC containers - resolve all command objects in one fell swoop
* No dependencies

## Examples ##

For each subcommand, implement a class that inherits the `ISubCommand` interface, which requires implementing one method (`Execute`) that is called to run the command. In your main function, create a `Parser` object and call `Parse(args)`, which will return the command to run. If no command is specified, the returned command will be a built-in `Help` command. Any errors (unknown command, for example) will throw a `CommandLineParserException`.

	using Yaclops;

    public class AddCommand : ISubCommand
    {
        [CommandLineOption(ShortName="n")]
        public bool DryRun { get; set; }

        public void Execute()
        {
            // Execute the command....
            Console.WriteLine("Add command!");
        }
    }


    static void Main(string[] args)
    {
        try
        {
            var parser = new CommandLineParser(new ISubCommand[] { new AddCommand(), new DiffCommand() });

            var command = parser.Parse(args);

            command.Execute();
        }
        catch (CommandLineParserException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


Creating the commands is a little awkward, but the intent is that an IoC container, such as AutoFac, will be used, which makes it much cleaner:

        static void Main(string[] args)
        {
            try
            {
                var container = CreateContainer();

                var parser = container.Resolve<CommandLineParser>();

                var command = parser.Parse(args);

                command.Execute();
            }
            catch (CommandLineParserException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception in main.");
                Console.WriteLine(ex);
            }

            if (Debugger.IsAttached)
            {
                Console.Write("<press ENTER to continue>");
                Console.ReadLine();
            }
        }



        private static IContainer CreateContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Command-line specific stuff
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => typeof(ISubCommand).IsAssignableFrom(t) && t.IsPublic)
                .SingleInstance()
                .As<ISubCommand>();

            builder.RegisterType<CommandLineParser>();

            return builder.Build();
        }

More examples forthcoming. The included Sample project has additional examples.


## Links ##

* [Other Libraries](OtherLibraries.md)
* [Todo List](ToDo.md)