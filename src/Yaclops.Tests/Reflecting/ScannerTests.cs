using System.Linq;
using NUnit.Framework;
using Shouldly;
using Yaclops.Attributes;
using Yaclops.Parsing;
using Yaclops.Reflecting;

namespace Yaclops.Tests.Reflecting
{
    [TestFixture]
    public class ScannerTests
    {
        private ParserConfiguration _configuration;
        private CommandScanner _scanner;


        [SetUp]
        public void BeforeEachTest()
        {
            _configuration = new ParserConfiguration();
            _scanner = new CommandScanner(_configuration);
        }



        [Test]
        public void CanScanEmptyCommand()
        {
            var subCommand = new EmptyCommand();
            _scanner.Scan(subCommand);

            _configuration.GlobalNamedParameters.ShouldBeEmpty();
            _configuration.Commands.Count().ShouldBe(1);
            _configuration.Commands.ShouldContain(x => x.Text == "empty");
        }



        [Test]
        public void CanScanNamedParameter()
        {
            var subCommand = new NamedParameterCommand();
            _scanner.Scan(subCommand);

            var command = _configuration.Commands.First();
            command.NamedParameters.ShouldNotBeEmpty();
            // TODO - verify parameter is correct
        }



        // ReSharper disable UnusedMember.Local
        private class EmptyCommand : ISubCommand
        {
            public void Execute() { }
        }


        private class NamedParameterCommand : ISubCommand
        {
            [NamedParameter]
            public string File { get; set; }

            public void Execute() { }
        }
        // ReSharper restore UnusedMember.Local
    }
}
