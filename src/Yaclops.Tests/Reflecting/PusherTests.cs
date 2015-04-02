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

        [Test]
        public void CanPushString()
        {
            ParserParameter param = new ParserNamedParameter("Name", typeof(string));
            ParseResult result = new ParseResult();
            result.AddCommandValue(param, "value");

            var pusher = new CommandPusher(result);

            var command = new StringCommand();

            pusher.Push(command);

            command.Name.ShouldBe("value");
        }


        [Test]
        public void CanPushBool()
        {
            ParserParameter param = new ParserNamedParameter("Add", typeof(bool));
            ParseResult result = new ParseResult();
            result.AddCommandValue(param, "true");

            var pusher = new CommandPusher(result);

            var command = new BoolCommand();

            pusher.Push(command);

            command.Add.ShouldBe(true);
        }


        private abstract class TestBase : ISubCommand
        {
            public void Execute() { }
        }


        // ReSharper disable UnusedAutoPropertyAccessor.Local
        private class StringCommand : TestBase
        {
            [NamedParameter]
            public string Name { get; set; }
        }

        private class BoolCommand : TestBase
        {
            [NamedParameter]
            public bool Add { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}
