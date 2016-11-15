using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Shouldly;
using Yaclops.Attributes;
using Yaclops.Reflecting;

#pragma warning disable 618


namespace Yaclops.Tests.Reflecting
{
    [TestFixture]
    public class ReflectedCommandTests
    {
        // These tests never actually instantiate the command, so just use object throughout
        private readonly Func<object> _factory = () => new object();


        [Test]
        public void SingleWordClassGetsProperVerbs()
        {
            var command = new ReflectedCommand<object>(typeof(Single), _factory);

            command.Verbs.ShouldBe(new[] { "single" });
        }


        [Test]
        public void SingleWordCommandGetsProperVerbs()
        {
            var command = new ReflectedCommand<object>(typeof(SingleCommand), _factory);

            command.Verbs.ShouldBe(new[] { "single" });
        }


        [Test]
        public void DoubleWordClassGetsProperVerbs()
        {
            var command = new ReflectedCommand<object>(typeof(TwoWords), _factory);

            command.Verbs.ShouldBe(new[] { "two", "words" });
        }


        [Test]
        public void NamedBoolParameterGetsPickedUp()
        {
            var command = new ReflectedCommand<object>(typeof(NamedBool), _factory);

            command.NamedParameters.Count.ShouldBe(1);
            var param = command.NamedParameters[0];
            param.PropertyName.ShouldBe("Flag");
            param.IsBool.ShouldBe(true);
            param.HasLongName("flag").ShouldBe(true);
        }



        [Test]
        public void NamedNullableBoolParameterGetsPickedUp()
        {
            var command = new ReflectedCommand<object>(typeof(NamedNullableBool), _factory);

            command.NamedParameters.Count.ShouldBe(1);
            var param = command.NamedParameters[0];
            param.PropertyName.ShouldBe("Flag");
            param.IsBool.ShouldBe(true);
            param.HasLongName("flag").ShouldBe(true);
        }



        [Test]
        public void NamedLongParameterGetsPickedUp()
        {
            var command = new ReflectedCommand<object>(typeof(NamedLong), _factory);

            command.NamedParameters.Count.ShouldBe(1);
            var param = command.NamedParameters[0];
            param.PropertyName.ShouldBe("Num");
            param.IsBool.ShouldBe(false);
            param.HasLongName("magic").ShouldBe(true);
            param.HasShortName("n").ShouldBe(true);
        }


        [Test]
        public void PositionalStringParameterGetsPickedUp()
        {
            var command = new ReflectedCommand<object>(typeof(PositionalString), _factory);

            command.PositionalParameters.Count.ShouldBe(1);
            var param = command.PositionalParameters[0];
            param.PropertyName.ShouldBe("Stuff");
            param.IsMandatory.ShouldBe(false);
            param.IsList.ShouldBe(false);
        }


        [Test]
        public void PositionalStringListParameterGetsPickedUp()
        {
            var command = new ReflectedCommand<object>(typeof(PositionalStringList), _factory);

            command.PositionalParameters.Count.ShouldBe(1);
            var param = command.PositionalParameters[0];
            param.PropertyName.ShouldBe("Stuffs");
            param.IsMandatory.ShouldBe(false);
            param.IsList.ShouldBe(true);
        }


        [Test]
        public void PositionalDoubleListParameterGetsPickedUp()
        {
            var command = new ReflectedCommand<object>(typeof(PositionalDoubleList), _factory);

            command.PositionalParameters.Count.ShouldBe(1);
            var param = command.PositionalParameters[0];
            param.PropertyName.ShouldBe("Fractions");
            param.IsMandatory.ShouldBe(false);
            param.IsList.ShouldBe(true);
        }


        [Test]
        public void RequiredParameterIsMarkedAsMandatory()      // For legacy support
        {
            var command = new ReflectedCommand<object>(typeof(PositionalRequired), _factory);

            command.PositionalParameters.Count.ShouldBe(1);
            var param = command.PositionalParameters[0];
            param.IsMandatory.ShouldBe(true);
        }


        [Test]
        public void MandatoryParameterIsMarkedAsMandatory()      // For legacy support
        {
            var command = new ReflectedCommand<object>(typeof(PositionalMandatory), _factory);

            command.PositionalParameters.Count.ShouldBe(1);
            var param = command.PositionalParameters[0];
            param.IsMandatory.ShouldBe(true);
        }


        [Test]
        public void DuplicateExplicitShortNamesThrows()
        {
            Should.Throw<CommandLineParserException>(() => new ReflectedCommand<object>(typeof (DuplicateExplicitShortNames), _factory));
        }

        [Test]
        public void DuplicateOldShortNamesThrows()
        {
            Should.Throw<CommandLineParserException>(() => new ReflectedCommand<object>(typeof (DuplicateOldShortNames), _factory));
        }


        [Test]
        public void DuplicateExplicitLongNamesThrows()
        {
            Should.Throw<CommandLineParserException>(() => new ReflectedCommand<object>(typeof(DuplicateExplicitLongNames), _factory));
        }

        [Test]
        public void DuplicateOldLongNamesThrows()
        {
            Should.Throw<CommandLineParserException>(() => new ReflectedCommand<object>(typeof(DuplicateOldLongNames), _factory));
        }

        [Test]
        public void DuplicateDefaultLongNamesThrows()
        {
            Should.Throw<CommandLineParserException>(() => new ReflectedCommand<object>(typeof(DuplicateDefaultLongNames), _factory));
        }


        // TODO - add test for conflict between default long name and attributed long name
        // TODO - add test for short name in NamedParameterAtt and ShortNameAtt


        #region Test Commands
        // ReSharper disable UnusedMember.Local

        private class Single { }
        private class SingleCommand { }
        private class TwoWords { }

        private class NamedBool
        {
            [NamedParameter]
            public bool Flag { get; set; }
        }

        private class NamedNullableBool
        {
            [NamedParameter]
            public bool? Flag { get; set; }
        }

        private class NamedLong
        {
            [NamedParameter(LongName = "magic", ShortName = "n")]
            public long Num { get; set; }
        }

        private class PositionalString
        {
            [PositionalParameter]
            public string Stuff { get; set; }
        }

        private class PositionalStringList
        {
            [PositionalParameter]
            public List<string> Stuffs { get; set; }
        }

        private class PositionalDoubleList
        {
            [PositionalParameter]
            public List<double> Fractions { get; set; }
        }

        private class PositionalRequired
        {
            [PositionalParameter, Required]
            public int Count { get; set; }
        }

        private class PositionalMandatory
        {
            [PositionalParameter, Mandatory]
            public int Count { get; set; }
        }

        private class DuplicateExplicitShortNames
        {
            [NamedParameter, ShortName("x")]
            public bool Foo { get; set; }

            [NamedParameter, ShortName("x")]
            public bool Bar { get; set; }
        }

        private class DuplicateOldShortNames
        {
            [NamedParameter(ShortName = "x")]
            public bool Foo { get; set; }

            [NamedParameter, ShortName("x")]
            public bool Bar { get; set; }
        }

        private class DuplicateExplicitLongNames
        {
            [NamedParameter, LongName("fred")]
            public bool Foo { get; set; }

            [NamedParameter, LongName("fred")]
            public bool Bar { get; set; }
        }

        private class DuplicateOldLongNames
        {
            [NamedParameter(LongName = "fred")]
            public bool Foo { get; set; }

            [NamedParameter, LongName("fred")]
            public bool Bar { get; set; }
        }

        private class DuplicateDefaultLongNames
        {
            [NamedParameter]
            public bool Fred { get; set; }

            [NamedParameter, LongName("fred")]
            public bool Bar { get; set; }
        }

        // ReSharper restore UnusedMember.Local
        #endregion
    }
}
