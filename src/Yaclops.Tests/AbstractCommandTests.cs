using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;

namespace Yaclops.Tests
{
    [TestFixture]
    public abstract class AbstractCommandTests
    {

        protected static void CanFindCommand(IEnumerable<ICommand> commands, Type expected, params string[] args)
        {
            var parser = new CommandLineParser(commands);

            var result = parser.Parse(args);

            result.ShouldBeOfType(expected);
        }




        protected class FunkyCommand : ICommand
        {
            public void Execute() { }
        }

        protected class PullCommand : ICommand
        {
            public void Execute() { }
        }

        protected class PushCommand : ICommand
        {
            public void Execute() { }
        }



        protected class FetchItemCommand : ICommand
        {
            public void Execute() { }
        }

        protected class FetchStickCommand : ICommand
        {
            public void Execute() { }
        }

        protected class PullItemCommand : ICommand
        {
            public void Execute() { }
        }

        protected class PushItemCommand : ICommand
        {
            public void Execute() { }
        }
    }
}
