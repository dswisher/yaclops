using NUnit.Framework;
using Shouldly;
using Yaclops.Reflecting;

namespace Yaclops.Tests.Reflecting
{
    [TestFixture]
    public class ReflectedCommandTests
    {

        [Test]
        public void SingleWordClassGetsProperVerbs()
        {
            var command = new ReflectedCommand<Single>(typeof(Single), () => new Single());

            command.Verbs.ShouldBe(new[] { "Single" });
        }


        [Test]
        public void SingleWordCommandGetsProperVerbs()
        {
            var command = new ReflectedCommand<SingleCommand>(typeof(SingleCommand), () => new SingleCommand());

            command.Verbs.ShouldBe(new[] { "Single" });
        }




        #region Test Commands

        public class Single { }
        public class SingleCommand { }

        #endregion
    }
}
