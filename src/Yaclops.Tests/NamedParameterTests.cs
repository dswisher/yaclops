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
            result.Flag.ShouldBe(true);
        }


        [Test]
        public void CannotSetFlagByPosition()
        {
            var parser = new CommandLineParser(new[] { new BoolCommand() });
            Should.Throw<CommandLineParserException>(() => parser.Parse(new[] { "bool", "p1", "true" }));
        }


        [Test]
        public void CanSetStringOptionByName()
        {
            var parser = new CommandLineParser(new[] { new StringCommand() });
            var result = (StringCommand)parser.Parse(new[] { "string", "--name", "fred" });
            result.Param1.ShouldBe(null);
            result.Name.ShouldBe("fred");
        }


        [Test]
        public void CanSetMultiWordOptionByDefaultLongName()
        {
            var parser = new CommandLineParser(new[] { new AddCommand() });
            var result = (AddCommand)parser.Parse(new[] { "add", "--dry-run" });
            result.DryRun.ShouldBe(true);
        }


        [Test]
        public void CanSetStringOptionByShortName()
        {
            var parser = new CommandLineParser(new[] { new StringCommand() });
            var result = (StringCommand)parser.Parse(new[] { "string", "-n", "fred" });
            result.Param1.ShouldBe(null);
            result.Name.ShouldBe("fred");
        }


        [Test]
        public void MustSpecifyValueForNamedParameter()
        {
            var parser = new CommandLineParser(new[] { new StringCommand() });
            Should.Throw<CommandLineParserException>(() => parser.Parse(new[] { "string", "-name" }));
        }


        [Test]
        public void CanSetLongOptionByName()
        {
            var parser = new CommandLineParser(new[] { new LongCommand() });
            var result = (LongCommand)parser.Parse(new[] { "long", "--value", "1337" });
            result.Param1.ShouldBe(null);
            result.Value.ShouldBe(1337);
        }


        [Test]
        public void CanSetOptionByLongNameOverride()
        {
            var parser = new CommandLineParser(new[] { new UserCommand() });
            var result = (UserCommand)parser.Parse(new[] { "user", "--add-user" });
            result.CreateUser.ShouldBe(true);
        }


        [Test]
        public void CannotSetOptionByDefaultLongNameWhenOverridden()
        {
            var parser = new CommandLineParser(new[] { new UserCommand() });
            Should.Throw<CommandLineParserException>(() => parser.Parse(new[] { "user", "--create-user" }));
        }


        #region Test Commands
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class BoolCommand : ISubCommand
        {
            [CommandLineOption]
            public string Param1 { get; set; }

            [CommandLineOption(ShortName = "flag")]
            public bool Flag { get; set; }

            public void Execute() { }
        }


        private class StringCommand : ISubCommand
        {
            [CommandLineOption]
            public string Param1 { get; set; }

            [CommandLineOption(ShortName = "n")]
            public string Name { get; set; }

            public void Execute() { }
        }


        private class LongCommand : ISubCommand
        {
            [CommandLineOption]
            public string Param1 { get; set; }

            [CommandLineOption]
            public long Value { get; set; }

            public void Execute() { }
        }


        private class AddCommand : ISubCommand
        {
            [CommandLineOption]
            public bool DryRun { get; set; }

            public void Execute() { }
        }


        private class UserCommand : ISubCommand
        {
            [CommandLineOption(LongName = "add-user")]
            public bool CreateUser { get; set; }

            public void Execute() { }
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Local
        #endregion
    }
}
