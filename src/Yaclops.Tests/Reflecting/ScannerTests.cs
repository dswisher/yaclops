using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using Yaclops.Attributes;
using Yaclops.Parsing.Configuration;
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
            param.HasShortName("x").ShouldBe(true);
            param.IsBool.ShouldBe(true);
        }



        [Test]
        public void LongNameAttributeOverridesClassNameForCommand()
        {
            var subCommand = new BlahCommand();
            _scanner.Scan(subCommand);

            _configuration.Commands.ShouldContain(x => x.Text == "monkey");
        }



        [Test]
        public void CanScanPositionalParameter()
        {
            var subCommand = new PositionalStringParameterCommand();
            _scanner.Scan(subCommand);

            var command = _configuration.Commands.First();

            var param = command.PositionalParameters.First();
            param.ShouldNotBe(null);
            param.Key.ShouldBe("Name");
            param.IsRequired.ShouldBe(false);
        }



        [Test]
        public void CanScanRequiredPositionalParameter()
        {
            var subCommand = new RequiredPositionalParameterCommand();
            _scanner.Scan(subCommand);

            var command = _configuration.Commands.First();

            var param = command.PositionalParameters.First();
            param.ShouldNotBe(null);
            param.Key.ShouldBe("Name");
            param.IsRequired.ShouldBe(true);
        }



        [Test]
        public void CanScanRequiredPositionalListParameter()
        {
            var subCommand = new RequiredPositionalListParameterCommand();
            _scanner.Scan(subCommand);

            var command = _configuration.Commands.First();

            var param = command.PositionalParameters.First();
            param.ShouldNotBe(null);
            param.Key.ShouldBe("Names");
            param.IsRequired.ShouldBe(true);
        }



        // ReSharper disable UnusedMember.Local
        private abstract class AbstractSubCommand : ISubCommand
        {
            public void Execute() { }
        }


        private class EmptyCommand : AbstractSubCommand
        {
        }


        [LongName("monkey")]
        private class BlahCommand : AbstractSubCommand
        {
        }


        private class NamedStringParameterCommand : AbstractSubCommand
        {
            [NamedParameter]
            public string File { get; set; }
        }


        private class NamedBoolParameterCommand : AbstractSubCommand
        {
            [NamedParameter(ShortName="x")]
            public bool DryRun { get; set; }
        }


        private class PositionalStringParameterCommand : AbstractSubCommand
        {
            [PositionalParameter]
            public string Name { get; set; }
        }


        private class RequiredPositionalParameterCommand : AbstractSubCommand
        {
            [PositionalParameter, Required]
            public string Name { get; set; }
        }


        private class RequiredPositionalListParameterCommand : AbstractSubCommand
        {
            [PositionalParameter, Required]
            public List<string> Names { get; set; }
        }
        // ReSharper restore UnusedMember.Local
    }
}
