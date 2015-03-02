using NUnit.Framework;
using Shouldly;

namespace Yaclops.Tests
{
    [TestFixture]
    public class NamedParameterTests
    {

        [Test]
        public void CanSetBooleanFlagByName()
        {
            var parser = new CommandLineParser(new[] { new BoolCommand() });
            var result = (BoolCommand)parser.Parse(new[] { "bool", "-flag" });
            result.Param1.ShouldBe(null);
            result.Param2.ShouldBe(true);
        }


        [Test]
        public void CannotSetFlagByPosition()
        {
            var parser = new CommandLineParser(new[] { new BoolCommand() });
            Should.Throw<CommandLineParserException>(() => parser.Parse(new[] { "bool", "p1", "true" }));
        }


        [Test]
        public void CanSetStringFlagByName()
        {
            var parser = new CommandLineParser(new[] { new StringCommand() });
            var result = (StringCommand)parser.Parse(new[] { "string", "-name", "fred" });
            result.Param1.ShouldBe(null);
            result.Param2.ShouldBe("fred");
        }


        [Test]
        public void MustSpecifyValueForNamedParameter()
        {
            var parser = new CommandLineParser(new[] { new StringCommand() });
            Should.Throw<CommandLineParserException>(() => parser.Parse(new[] { "string", "-name" }));
        }


        [Test]
        public void CanSetLongFlagByName()
        {
            var parser = new CommandLineParser(new[] { new LongCommand() });
            var result = (LongCommand)parser.Parse(new[] { "long", "-value", "1337" });
            result.Param1.ShouldBe(null);
            result.Param2.ShouldBe(1337);
        }

        
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        private class BoolCommand : ICommand
        {
            [CommandLineParameter]
            public string Param1 { get; set; }

            [CommandLineParameter(Name = "flag")]
            public bool Param2 { get; set; }

            public void Execute() { }
        }


        private class StringCommand : ICommand
        {
            [CommandLineParameter]
            public string Param1 { get; set; }

            [CommandLineParameter(Name = "name")]
            public string Param2 { get; set; }

            public void Execute() { }            
        }


        private class LongCommand : ICommand
        {
            [CommandLineParameter]
            public string Param1 { get; set; }

            [CommandLineParameter(Name = "value")]
            public long Param2 { get; set; }

            public void Execute() { }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}
