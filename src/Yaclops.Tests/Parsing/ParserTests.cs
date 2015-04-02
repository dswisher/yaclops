using System.Linq;
using NUnit.Framework;
using Shouldly;
using Yaclops.Parsing;

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

            var result = DoParse("add");
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
            _config.AddNamedParameter("File", typeof(string)).ShortName("f");

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
            _config.AddNamedParameter("File", typeof(string));

            var result = DoParse("--file bar.txt");

            result.Command.ShouldBe(null);

            var value = result.GlobalValues.FirstOrDefault(x => x.Name == "File");

            value.ShouldNotBe(null);
            value.Name.ShouldBe("File");
            value.Value.ShouldBe("bar.txt");
        }



        [Test]
        public void CommandSpecificParameterNotAvailableGlobally()
        {
            _config.AddCommand("add").AddNamedParameter("File", typeof(string));

            var result = DoParse("--file foo.txt add");
            result.Command.ShouldBe(null);
            result.Errors.ShouldContain(x => x.Contains("file"));
        }



        [Test]
        public void CanParseCommandParameterByLongName()
        {
            var command = _config.AddCommand("add");
            command.AddNamedParameter("File", typeof(string));

            var result = DoParse("add --file foo.txt");
            result.Command.ShouldBe(command);
            result.GlobalValues.ShouldBeEmpty();

            var value = result.CommandValues.FirstOrDefault(x => x.Name == "File");

            value.ShouldNotBe(null);
            value.Name.ShouldBe("File");
            value.Value.ShouldBe("foo.txt");
        }


        // TODO - test global long-name AFTER command: add --file foo.txt where --file is global, and not command-specific


        private ParseResult DoParse(string text)
        {
            Parser parser = new Parser(_config);

            return parser.Parse(text);
        }
    }
}
