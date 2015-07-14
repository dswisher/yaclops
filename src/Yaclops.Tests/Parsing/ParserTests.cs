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



        [Test]
        public void MissingRequiredStringParamThrows()
        {
            var root = _builder.ExternalCommand("funky")
                .WithPositionalString("Name", true)
                .Root;

            var parser = new Parser(root);

            Should.Throw<CommandLineParserException>(() => parser.Parse("funky"));
        }



        [Test]
        public void SpecifiedRequiredStringParamIsSet()
        {
            var root = _builder.ExternalCommand("funky")
                .WithPositionalString("Name", true)
                .Root;

            var parser = new Parser(root);

            var result = parser.Parse("funky foo");

            result.Kind.ShouldBe(ParseResultKind.ExternalCommand);

            result.PositionalParameters.Count.ShouldBe(1);
            result.PositionalParameters[0].Parameter.PropertyName.ShouldBe("Name");
            result.PositionalParameters[0].Values.ShouldBe(new[] { "foo" });
        }


        // TODO - add test for required List<string>
    }
}
