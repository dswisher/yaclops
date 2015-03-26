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
            const string subcommand = "add";

            _config.AddCommand(subcommand);

            DoParse(subcommand).Command.ShouldBe(subcommand);
        }



        [Test]
        public void CanFindTwoWordSubcommand()
        {
            const string subcommand = "bisect start";

            _config.AddCommand(subcommand);

            DoParse(subcommand).Command.ShouldBe(subcommand);
        }



        [Test]
        public void UnknownSubcommandReturnsNull()
        {
            _config.AddCommand("pull");
            _config.AddCommand("fetch");

            DoParse("add").Command.ShouldBe(null);
        }



        [Test, Ignore("Get this working!")]
        public void CanFindCommandByAlias()
        {
            _config.AddCommand("help").AddAlias("--help");

            DoParse("--help").Command.ShouldBe("help");
        }



        private ParseResult DoParse(string text)
        {
            Parser parser = new Parser(_config);

            return parser.Parse(text);
        }
    }
}
