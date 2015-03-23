using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Shouldly;

namespace Yaclops.Tests
{
    [TestFixture]
    public abstract class AbstractCommandTests
    {

        protected static void CanFindCommand(IEnumerable<ISubCommand> commands, Type expected, params string[] args)
        {
            var parser = new CommandLineParser(commands);

            var result = parser.Parse(args, false);

            result.ShouldBeOfType(expected);
        }




        protected class FunkyCommand : ISubCommand
        {
            public void Execute() { }
        }

        protected class PullCommand : ISubCommand
        {
            public void Execute() { }
        }

        protected class PushCommand : ISubCommand
        {
            public void Execute() { }
        }

        protected class RequiredCommand : ISubCommand
        {
            [CommandLineParameter, Required]
            public string FileName { get; set; }

            public void Execute() { }
        }


        protected class FetchItemCommand : ISubCommand
        {
            public void Execute() { }
        }

        protected class FetchStickCommand : ISubCommand
        {
            public void Execute() { }
        }

        protected class PullItemCommand : ISubCommand
        {
            public void Execute() { }
        }

        protected class PushItemCommand : ISubCommand
        {
            public void Execute() { }
        }
    }
}
