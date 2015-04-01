using NUnit.Framework;
using Shouldly;
using Yaclops.Attributes;
using Yaclops.Parsing;
using Yaclops.Reflecting;

namespace Yaclops.Tests.Reflecting
{
    [TestFixture]
    public class PusherTests
    {

        [Test, Ignore("Get this working! TODO!")]
        public void CanPushString()
        {
            ParserParameter param = new ParserNamedParameter("name");
            ParseResult result = new ParseResult();
            result.AddCommandValue(param, "value");

            var pusher = new CommandPusher(result);

            var command = new StringCommand();

            pusher.Push(command);

            // TODO - get this working
            command.Name.ShouldBe("value");
        }


        private abstract class TestBase : ISubCommand
        {
            public void Execute() { }
        }


        private class StringCommand : TestBase
        {
            [NamedParameter]
            public string Name { get; set; }
        }
    }
}
