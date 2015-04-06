using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using Yaclops.Attributes;
using Yaclops.Parsing;
using Yaclops.Parsing.Configuration;
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


        [Test]
        public void CanPushStringList()
        {
            ParserParameter param = new ParserPositionalParameter("Names", true);
            ParseResult result = new ParseResult();

            result.AddCommandListValue(param, "Fred");
            result.AddCommandListValue(param, "Barney");

            var pusher = new CommandPusher(result);

            var command = new ListCommand();

            pusher.Push(command);

            command.Names.ShouldContain(x => x == "Fred");
            command.Names.ShouldContain(x => x == "Barney");
        }


        private abstract class TestBase : ISubCommand
        {
            public void Execute() { }
        }


        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable CollectionNeverUpdated.Local
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

        private class ListCommand : TestBase
        {
            public ListCommand()
            {
                // TODO - the parser should set this to an empty list, methinks
                Names = new List<string>();
            }

            [PositionalParameter]
            public List<string> Names { get; set; }
        }
        // ReSharper restore CollectionNeverUpdated.Local
        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}
