using System;
using NUnit.Framework;
using Shouldly;
using Yaclops.Attributes;
using Yaclops.Reflecting;

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

            command.Verbs.ShouldBe(new[] { "Single" });
        }


        [Test]
        public void SingleWordCommandGetsProperVerbs()
        {
            var command = new ReflectedCommand<object>(typeof(SingleCommand), _factory);

            command.Verbs.ShouldBe(new[] { "Single" });
        }


        [Test]
        public void DoubleWordClassGetsProperVerbs()
        {
            var command = new ReflectedCommand<object>(typeof(TwoWords), _factory);

            command.Verbs.ShouldBe(new[] { "Two", "Words" });
        }


        [Test]
        public void NamedBoolParameterGetsPickedUp()
        {
            var command = new ReflectedCommand<object>(typeof(NamedBool), _factory);

            command.NamedParameters.Count.ShouldBe(1);
            var param = command.NamedParameters[0];
            param.PropertyName.ShouldBe("Flag");
            param.IsBool.ShouldBe(true);
        }



        [Test]
        public void NamedLongParameterGetsPickedUp()
        {
            var command = new ReflectedCommand<object>(typeof(NamedLong), _factory);

            command.NamedParameters.Count.ShouldBe(1);
            var param = command.NamedParameters[0];
            param.PropertyName.ShouldBe("Num");
            param.IsBool.ShouldBe(false);
        }



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

        private class NamedLong
        {
            [NamedParameter]
            public long Num { get; set; }
        }

        // ReSharper restore UnusedMember.Local
        #endregion
    }
}
