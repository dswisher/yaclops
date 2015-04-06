using NUnit.Framework;
using Shouldly;
using Yaclops.Parsing;
using Yaclops.Parsing.Configuration;

namespace Yaclops.Tests.Parsing
{
    [TestFixture]
    public class ParserConfigurationTests
    {
        private ParserConfiguration _config;


        [SetUp]
        public void BeforeEachTest()
        {
            _config = new ParserConfiguration();
        }


        [Test]
        public void AddingDuplicateCommandThrows()
        {
            _config.AddCommand("foo");
            Should.Throw<ParserConfigurationException>(() => _config.AddCommand("foo"));
        }


        [Test]
        public void NamedParameterGetsDefaultLongName()
        {
            var param = _config.AddNamedParameter("File");

            param.HasLongName("file").ShouldBe(true);
        }


        [Test]
        public void ExplicitLongNameOverridesDefault()
        {
            var param = _config.AddNamedParameter("File").AddLongName("Monkey");

            param.HasLongName("file").ShouldBe(false);
            param.HasLongName("monkey").ShouldBe(false);
            param.HasLongName("Monkey").ShouldBe(true);
        }

        // TODO - how to disable long name?

        // TODO - add test to verify exception if required positional parameter follows optional positional parameter
    }
}
