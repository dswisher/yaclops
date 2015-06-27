using NUnit.Framework;
using Shouldly;
using Yaclops.Model;
using Yaclops.Tests.Mocks;

namespace Yaclops.Tests.Model
{
    [TestFixture]
    public class ModelBuilderTests
    {
        private ModelBuilder _builder;


        [SetUp]
        public void BeforeEachTest()
        {
            _builder = new ModelBuilder();
        }


        [Test]
        public void SingleVerb()
        {
            const string verb = "funky";

            _builder.AddType(MockReflectedCommand.FromVerbs(verb));

            _builder.Root.Nodes.Count.ShouldBe(1);

            var node = _builder.Root.Nodes[0];

            node.ShouldBeOfType<Command<object>>();
            node.Verb.ShouldBe(verb);
            ((Command<object>)node).Factory.ShouldNotBe(null);
        }



        [Test]
        public void DoubleVerb()
        {
            const string verb1 = "funky";
            const string verb2 = "fresh";

            _builder.AddType(MockReflectedCommand.FromVerbs(verb1, verb2));

            _builder.Root.Nodes.Count.ShouldBe(1);

            var node = _builder.Root.Nodes[0];

            node.ShouldBeOfType<CommandGroup>();
            node.Verb.ShouldBe(verb1);
            ((CommandGroup)node).Nodes.Count.ShouldBe(1);
            node = ((CommandGroup)node).Nodes[0];

            node.ShouldBeOfType<Command<object>>();
            node.Verb.ShouldBe(verb2);
            ((Command<object>)node).Factory.ShouldNotBe(null);
        }
    }
}
