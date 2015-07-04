using NUnit.Framework;
using Shouldly;
using Yaclops.Extensions;

namespace Yaclops.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionTests
    {

        [Test]
        public void DecamelOneWordLower()
        {
            "foo".Decamel().ShouldBe(new[] { "foo" });
        }


        [Test]
        public void DecamelOneWordUpper()
        {
            "Foo".Decamel().ShouldBe(new[] { "foo" });
        }


        [Test]
        public void DecamelTwoWords()
        {
            "FooBar".Decamel().ShouldBe(new[] { "foo", "bar" });
        }
    }
}
