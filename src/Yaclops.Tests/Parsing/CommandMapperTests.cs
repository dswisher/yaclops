using NUnit.Framework;
using Shouldly;
using Yaclops.Parsing;
using Yaclops.Parsing.Configuration;

namespace Yaclops.Tests.Parsing
{
    [TestFixture]
    public class CommandMapperTests
    {

        [Test]
        public void CanFindOneAndOnly()
        {
            ParserCommand command = new ParserCommand("add");
            var mapper = new CommandMapper(new[] { command });
            mapper.Advance("add");

            mapper.State.ShouldBe(MapperState.Accepted);
            mapper.Command.ShouldBe(command);
        }


        [Test]
        public void CanFindSecondOfTwo()
        {
            ParserCommand command1 = new ParserCommand("add");
            ParserCommand command2 = new ParserCommand("remove");
            var mapper = new CommandMapper(new[] { command1, command2 });
            mapper.Advance("remove");

            mapper.State.ShouldBe(MapperState.Accepted);
            mapper.Command.ShouldBe(command2);
        }


        [Test]
        public void CanFindTwoWordCommand()
        {
            ParserCommand command1 = new ParserCommand("add file");
            ParserCommand command2 = new ParserCommand("add comment");
            var mapper = new CommandMapper(new[] { command1, command2 });
            mapper.Advance("add");
            mapper.State.ShouldBe(MapperState.Initial);

            mapper.Advance("comment");
            mapper.State.ShouldBe(MapperState.Accepted);
            mapper.Command.ShouldBe(command2);
        }


        [Test]
        public void InvalidFirstWordRejected()
        {
            ParserCommand command1 = new ParserCommand("add file");
            ParserCommand command2 = new ParserCommand("add comment");
            var mapper = new CommandMapper(new[] { command1, command2 });
            mapper.Advance("remove");

            mapper.State.ShouldBe(MapperState.Rejected);
        }


        [Test]
        public void InvalidSecondWordRejected()
        {
            ParserCommand command1 = new ParserCommand("add file");
            ParserCommand command2 = new ParserCommand("add comment");
            var mapper = new CommandMapper(new[] { command1, command2 });
            mapper.Advance("add");
            mapper.State.ShouldBe(MapperState.Initial);

            mapper.Advance("junk");
            mapper.State.ShouldBe(MapperState.Rejected);
        }


        [Test]
        public void AmbiguousTwoWordThrows()
        {
            ParserCommand command1 = new ParserCommand("add");
            ParserCommand command2 = new ParserCommand("add file");

            // ReSharper disable once ObjectCreationAsStatement
            Should.Throw<ParserConfigurationException>(() => new CommandMapper(new[] { command1, command2 }));
        }


        [Test]
        public void AmbiguousOneWordThrows()
        {
            ParserCommand command1 = new ParserCommand("add file");
            ParserCommand command2 = new ParserCommand("add");

            // ReSharper disable once ObjectCreationAsStatement
            Should.Throw<ParserConfigurationException>(() => new CommandMapper(new[] { command1, command2 }));
        }
    }
}
