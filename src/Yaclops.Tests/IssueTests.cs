using NUnit.Framework;
using Shouldly;

namespace Yaclops.Tests
{
    [TestFixture]
    public class IssueTests
    {
        // Tests to reproduce reported/noticed issues if they don't fit elsewhere

        [Test, Ignore("Need a way to handle 'global' options - those that appear before a command")]
        public void UnknownCommand()
        {
            var parser = new CommandLineParser(new[] { new LoadCommand() });
            var result = (LoadCommand)parser.Parse(new[] { "--max-rows", "3", "load" });
            result.MaxRows.ShouldBe(3);
        }



        #region Test Commands
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class LoadCommand : ISubCommand
        {
            [CommandLineOption(ShortName = "m")]
            public int MaxRows { get; set; }

            public void Execute()
            {
            }
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Local
        #endregion
    }
}
