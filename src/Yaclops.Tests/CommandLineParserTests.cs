using NUnit.Framework;
using Shouldly;

namespace Yaclops.Tests
{
    [TestFixture]
    public class CommandLineParserTests
    {

        [Test]
        public void ParsingWithoutCommandsThrows()
        {
            var parser = new CommandLineParser();
            Should.Throw<CommandLineParserException>(() => parser.Parse("funky blue"));
        }



    }
}
