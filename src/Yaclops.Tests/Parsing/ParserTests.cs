using NUnit.Framework;
using Shouldly;
using Yaclops.Parsing;
using Yaclops.Tests.Mocks;

namespace Yaclops.Tests.Parsing
{
    [TestFixture]
    public class ParserTests
    {
        private CommandBuilder _builder;


        [SetUp]
        public void BeforeEachTest()
        {
            _builder = new CommandBuilder();
        }


        [Test]
        public void BoolDoesNotGobblePositional()
        {
            var root = _builder.ExternalCommand("funky")
                .WithNamedBool("Test")
                .WithPositionalString("Name")
                .Root;

            var parser = new Parser(root);

            var result = parser.Parse("funky -t bar");

            result.Kind.ShouldBe(ParseResultKind.ExternalCommand);

            // TODO - can _builder help validate parameters?
            result.NamedParameters.Count.ShouldBe(1);
            result.NamedParameters[0].Parameter.PropertyName.ShouldBe("Test");
            result.NamedParameters[0].Value.ShouldBe(true.ToString());

            result.PositionalParameters.Count.ShouldBe(1);
            result.PositionalParameters[0].Parameter.PropertyName.ShouldBe("Name");
            result.PositionalParameters[0].Values.ShouldBe(new[] { "bar" });
        }

    }
}
