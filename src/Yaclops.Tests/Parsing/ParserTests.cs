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
            _config.AddNamedParameter("File").ShortName("f");

            var result = DoParse("-f foo.txt");

            result.Command.ShouldBe(null);

            var value = result.GlobalValues.FirstOrDefault(x => x.Name == "File");

            value.ShouldNotBe(null);
            value.Name.ShouldBe("File");
            value.Value.ShouldBe("foo.txt");
        }



        private ParseResult DoParse(string text)
        {
            Parser parser = new Parser(_config);

            return parser.Parse(text);
        }
    }
}
