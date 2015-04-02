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
        public void CanScanNamedStringParameter()
        {
            var subCommand = new NamedStringParameterCommand();
            _scanner.Scan(subCommand);

            var command = _configuration.Commands.First();
            var param = command.NamedParameters.First();

            param.Key.ShouldBe("File");
            param.HasLongName("file").ShouldBe(true);
            param.IsBool.ShouldBe(false);
        }



        [Test]
        public void CanScanNamedBoolParameter()
        {
            var subCommand = new NamedBoolParameterCommand();
            _scanner.Scan(subCommand);

            var command = _configuration.Commands.First();
            var param = command.NamedParameters.First();

            param.Key.ShouldBe("DryRun");
            param.HasLongName("dry-run").ShouldBe(true);
            param.IsBool.ShouldBe(true);
        }



        // ReSharper disable UnusedMember.Local
        private abstract class AbstractSubCommand : ISubCommand
        {
            public void Execute() { }
        }


        private class EmptyCommand : AbstractSubCommand
        {
        }


        private class NamedStringParameterCommand : AbstractSubCommand
        {
            [NamedParameter]
            public string File { get; set; }
        }


        private class NamedBoolParameterCommand : AbstractSubCommand
        {
            [NamedParameter]
            public bool DryRun { get; set; }
        }
        // ReSharper restore UnusedMember.Local
    }
}
