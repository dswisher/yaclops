using System.Linq;
using NUnit.Framework;
using Shouldly;
using Yaclops.Parsing;
using Yaclops.Parsing.Configuration;

namespace Yaclops.Tests.Parsing
{
    [TestFixture]
    public class ParserTests
    {
        private ParserConfiguration _config;


        [SetUp]
        public void BeforeEachTest()
        {
            _config = new ParserConfiguration();
        }


        [Test]
        public void CanFindOneWordSubcommand()
        {
            const string commandText = "add";

            var command = _config.AddCommand(commandText);

            DoParse(commandText).Command.ShouldBe(command);
        }



        [Test]
        public void CanFindTwoWordSubcommand()
        {
            const string commandText = "bisect start";

            var command = _config.AddCommand(commandText);

            DoParse(commandText).Command.ShouldBe(command);
        }



        [Test]
        public void UnknownSubcommandReturnsNullCommand()
        {
            _config.AddCommand("pull");
            _config.AddCommand("fetch");

            var result = DoBadParse("add");
            result.Command.ShouldBe(null);
            result.Errors.ShouldContain(x => x.Contains("add"));
        }



        [Test]
        public void CanFindCommandByAlias()
        {
            var command = _config.AddCommand("help").AddAlias("--help");

            DoParse("--help").Command.ShouldBe(command);
        }



        [Test]
        public void NoArgsReturnsDefaultCommand()
        {
            const string commandText = "add";

            _config.AddCommand("foo");
            var command = _config.AddCommand(commandText);
            _config.AddCommand("bar");
            _config.DefaultCommand = command;

            DoParse(null).Command.ShouldBe(command);
            DoParse("").Command.ShouldBe(command);
        }



        [Test]
        public void CanParseGlobalNamedParameterByShortName()
        {
            _config.AddNamedParameter("File").AddShortName("f");

            var result = DoParse("-f foo.txt");

            result.Command.ShouldBe(null);

            var value = result.GlobalValues.FirstOrDefault(x => x.Name == "File");

            value.ShouldNotBe(null);
            value.Name.ShouldBe("File");
            value.Value.ShouldBe("foo.txt");
        }



        [Test]
        public void CanParseGlobalNamedParameterByLongName()
        {
            _config.AddNamedParameter("File");

            var result = DoParse("--file bar.txt");

            result.Command.ShouldBe(null);

            var value = result.GlobalValues.FirstOrDefault(x => x.Name == "File");

            value.ShouldNotBe(null);
            value.Name.ShouldBe("File");
            value.Value.ShouldBe("bar.txt");
        }



        [Test]
        public void CanParseGlobalNamedBoolByShortName()
        {
            _config.AddNamedParameter("Verbose", true).AddShortName("v");

            var result = DoParse("-v");

            result.Command.ShouldBe(null);

            var value = result.GlobalValues.FirstOrDefault(x => x.Name == "Verbose");

            value.ShouldNotBe(null);
            value.Value.ShouldBe("true");
        }



        [Test]
        public void CanParseGlobalNamedBoolByLongName()
        {
            _config.AddNamedParameter("Verbose", true).AddShortName("v");

            var result = DoParse("--verbose");

            result.Command.ShouldBe(null);

            var value = result.GlobalValues.FirstOrDefault(x => x.Name == "Verbose");

            value.ShouldNotBe(null);
            value.Value.ShouldBe("true");
        }



        [Test]
        public void CommandSpecificParameterNotAvailableGlobally()
        {
            _config.AddCommand("add").AddNamedParameter("File");

            var result = DoBadParse("--file foo.txt add");
            result.Command.ShouldBe(null);
            result.Errors.ShouldContain(x => x.Contains("file"));
        }



        [Test]
        public void CanParseCommandStringParameterByLongName()
        {
            var command = _config.AddCommand("add");
            command.AddNamedParameter("File");

            var result = DoParse("add --file foo.txt");

            result.Command.ShouldBe(command);
            result.GlobalValues.ShouldBeEmpty();

            var value = result.CommandValues.FirstOrDefault(x => x.Name == "File");

            value.ShouldNotBe(null);
            value.Name.ShouldBe("File");
            value.Value.ShouldBe("foo.txt");
        }


        [Test]
        public void CanParseCommandStringParameterByShortName()
        {
            var command = _config.AddCommand("add");
            command.AddNamedParameter("File").AddShortName("z");

            var result = DoParse("add -z foo.txt");

            result.Command.ShouldBe(command);
            result.GlobalValues.ShouldBeEmpty();

            var value = result.CommandValues.FirstOrDefault(x => x.Name == "File");

            value.ShouldNotBe(null);
            value.Name.ShouldBe("File");
            value.Value.ShouldBe("foo.txt");
        }


        [Test]
        public void CanParseCommandBoolByLongName()
        {
            var command = _config.AddCommand("unzip");
            command.AddNamedParameter("List", true);

            var result = DoParse("unzip --list");

            var value = result.CommandValues.FirstOrDefault(x => x.Name == "List");

            value.ShouldNotBe(null);
            value.Value.ShouldBe("true");
        }


        [Test]
        public void CanParseCommandBoolByShortName()
        {
            var command = _config.AddCommand("unzip");
            command.AddNamedParameter("List", true).AddShortName("l");

            var result = DoParse("unzip -l");

            var value = result.CommandValues.FirstOrDefault(x => x.Name == "List");

            value.ShouldNotBe(null);
            value.Value.ShouldBe("true");
        }


        [Test]
        public void CanParseCommandPositionalString()
        {
            var command = _config.AddCommand("unzip");
            command.AddPositionalParameter("File", false);

            var result = DoParse("unzip foo.txt");

            var value = result.CommandValues.FirstOrDefault(x => x.Name == "File");

            value.ShouldNotBe(null);
            value.Value.ShouldBe("foo.txt");
        }



        [Test]
        public void ExtraPositionalValueReturnsError()
        {
            var command = _config.AddCommand("unzip");
            command.AddPositionalParameter("File", false);

            var result = DoBadParse("unzip foo.txt bar.txt");

            result.Errors.ShouldContain(x => x.ToLower().Contains("unexpected"));
        }


        [Test]
        public void CanParseCommandPositionalList()
        {
            var command = _config.AddCommand("add");
            command.AddPositionalParameter("Files", true);

            var result = DoParse("add foo.txt bar.txt");

            var value = result.CommandListValues.FirstOrDefault(x => x.Name == "Files");

            value.ShouldNotBe(null);
            value.Values.ShouldContain("foo.txt");
            value.Values.ShouldContain("bar.txt");
        }


        [Test]
        public void CanParseCommandBoolByShortNameAfterCommand()
        {
            _config.AddCommand("run");
            _config.AddNamedParameter("Help", true).AddShortName("h");

            var result = DoParse("run -h");

            var value = result.GlobalValues.FirstOrDefault(x => x.Name == "Help");

            value.ShouldNotBe(null);
            value.Value.ShouldBe("true");
        }


        [Test]
        public void CanParseCommandBoolByLongNameAfterCommand()
        {
            _config.AddCommand("run");
            _config.AddNamedParameter("Help", true).AddShortName("h");

            var result = DoParse("run --help");

            var value = result.GlobalValues.FirstOrDefault(x => x.Name == "Help");

            value.ShouldNotBe(null);
            value.Value.ShouldBe("true");
        }



        [Test]
        public void EnumerableOverloadCanHandleParameterWithSpaces()
        {
            var command = _config.AddCommand("exit");
            command.AddNamedParameter("Speed");

            Parser parser = new Parser(_config);

            var result = parser.Parse(new[] { "exit", "--speed", "super fast" });

            result.Errors.ShouldBeEmpty();

            var value = result.CommandValues.FirstOrDefault(x => x.Name == "Speed");

            value.ShouldNotBe(null);
            value.Value.ShouldBe("super fast");
        }


        // TODO - test global long-name AFTER command: add --file foo.txt where --file is global, and not command-specific


        private ParseResult DoParse(string text)
        {
            Parser parser = new Parser(_config);

            var result = parser.Parse(text);

            result.Errors.ShouldBeEmpty();

            return result;
        }


        private ParseResult DoBadParse(string text)
        {
            Parser parser = new Parser(_config);

            return parser.Parse(text);
        }
    }
}
