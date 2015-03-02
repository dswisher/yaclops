using NUnit.Framework;
using Shouldly;

namespace Yaclops.Tests
{
    [TestFixture]
    public class TwoWordCommandTests : AbstractCommandTests
    {
        private readonly ICommand[] _fetchItemCommand = { new FetchItemCommand() };



        [Test]
        public void ExactMatchOfLonelyCommand()
        {
            CanFindCommand(_fetchItemCommand, _fetchItemCommand[0].GetType(), "fetch", "item");
        }



        public void OneWordDoesNotMatchTwo()
        {
            var parser = new CommandLineParser(_fetchItemCommand);

            Should.Throw<CommandLineParserException>(() => parser.Parse(new[] { "fetch" }));
        }

    }
}
