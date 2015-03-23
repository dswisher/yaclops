using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Shouldly;


namespace Yaclops.Tests
{
    [TestFixture]
    public class PositionalParameterTests
    {

        [Test]
        public void CanSetOneStringParameter()
        {
            var parser = new CommandLineParser(new[] { new StringCommand() });
            var result = (StringCommand)parser.Parse(new[] { "string", "ping" });
            result.Param1.ShouldBe("ping");
            result.Param2.ShouldBe(null);
        }


        [Test]
        public void CanSetTwoStringParameters()
        {
            var parser = new CommandLineParser(new[] { new StringCommand() });
            var result = (StringCommand)parser.Parse(new[] { "string", "ping", "pong" });
            result.Param1.ShouldBe("ping");
            result.Param2.ShouldBe("pong");
        }


        [Test]
        public void MustSpecifyValueForRequiredStringParameter()
        {
            var parser = new CommandLineParser(new[] { new StringCommand() });
            Should.Throw<CommandLineParserException>(() => parser.Parse(new[] { "string" }));
        }


        [Test]
        public void CanSetIntParameter()
        {
            var parser = new CommandLineParser(new[] { new IntCommand() });
            var result = (IntCommand)parser.Parse(new[] { "int", "123" });
            result.Param.ShouldBe(123);
        }


        [Test]
        public void MustSpecifyValueForRequiredIntParameter()
        {
            var parser = new CommandLineParser(new[] { new IntCommand() });
            Should.Throw<CommandLineParserException>(() => parser.Parse(new[] { "int" }));
        }


        [Test]
        public void CanSetListParameterOnce()
        {
            var parser = new CommandLineParser(new[] { new ListCommand() });
            var result = (ListCommand)parser.Parse(new[] { "list", "one" });
            result.Param.ShouldContain("one");
        }


        [Test]
        public void CanSetListParameterTwice()
        {
            var parser = new CommandLineParser(new[] { new ListCommand() });
            var result = (ListCommand)parser.Parse(new[] { "list", "one", "two" });
            result.Param.ShouldContain("one");
            result.Param.ShouldContain("two");
        }


        [Test]
        public void CanSetRequiredListParameterOnce()
        {
            var parser = new CommandLineParser(new[] { new OtherCommand() });
            var result = (OtherCommand)parser.Parse(new[] { "other", "one" });
            result.Param.ShouldContain("one");
        }



        // TODO - custom types - a filter object, say


        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable CollectionNeverUpdated.Local
        private class StringCommand : ISubCommand
        {
            [CommandLineParameter, Required]
            public string Param1 { get; set; }

            [CommandLineParameter]
            public string Param2 { get; set; }

            public void Execute() { }
        }


        private class IntCommand : ISubCommand
        {
            [CommandLineParameter, Required]
            public int Param { get; set; }

            public void Execute() { }
        }


        private class ListCommand : ISubCommand
        {
            [CommandLineParameter]
            public List<string> Param { get; private set; }

            public void Execute() { }
        }


        private class OtherCommand : ISubCommand
        {
            [CommandLineParameter, Required]
            public List<string> Param { get; private set; }

            public void Execute() { }
        }
        // ReSharper restore CollectionNeverUpdated.Local
        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}
