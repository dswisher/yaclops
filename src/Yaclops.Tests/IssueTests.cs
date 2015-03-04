using NUnit.Framework;
using Shouldly;

namespace Yaclops.Tests
{
    [TestFixture]
    public class IssueTests
    {
        // Tests to reproduce reported issues

        [Test, Ignore("How does this related to global flags?")]
        public void UnknownCommand()
        {
            var parser = new CommandLineParser(new[] { new LoadCommand() });
            var result = (LoadCommand)parser.Parse(new[] { "--max-rows", "3", "load" });
            result.MaxRows.ShouldBe(3);
        }



        #region Test Commands

        private class LoadCommand : ISubCommand
        {
            [CommandLineOption(ShortName = "m")]
            public int MaxRows { get; set; }

            public void Execute()
            {
            }
        }

        #endregion
    }
}
