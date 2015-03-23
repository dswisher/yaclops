using NUnit.Framework;
using Shouldly;

namespace Yaclops.Tests
{
    [TestFixture]
    public class OneWordCommandTests : AbstractCommandTests
    {
        private readonly ISubCommand[] _funkyCommand = { new FunkyCommand() };
        private readonly ISubCommand[] _requiredCommand = { new RequiredCommand() };
        private readonly ISubCommand[] _threeCommands = { new FunkyCommand(), new PullCommand(), new PushCommand() };



        [Test]
        public void ExactMatchOfLonelyCommand()
        {
            CanFindCommand(_funkyCommand, _funkyCommand[0].GetType(), "funky");
        }



        [Test]
        public void CanPickOutFirstCommandByName()
        {
            CanFindCommand(_threeCommands, _threeCommands[0].GetType(), "funky");
        }



        [Test]
        public void CanPickOutMiddleCommandByName()
        {
            CanFindCommand(_threeCommands, _threeCommands[1].GetType(), "pull");
        }



        [Test]
        public void CanPickOutLastCommandByName()
        {
            CanFindCommand(_threeCommands, _threeCommands[2].GetType(), "push");
        }



        [Test]
        public void UnknownCommandThrows()
        {
            var parser = new CommandLineParser(_threeCommands);

            Should.Throw<CommandLineParserException>(() => parser.Parse(new[] { "xyzzy" }));
        }


        [Test]
        public void CanPickOutCommandWithRequiredParameter()
        {
            CanFindCommand(_requiredCommand, _requiredCommand[0].GetType(), "required");
        }


        // TODO - parser w/o any commands should throw
        // TODO - allow abbreviation - "f" should give funky, "pu" should throw, "pul" should give pull, etc.
    }
}
