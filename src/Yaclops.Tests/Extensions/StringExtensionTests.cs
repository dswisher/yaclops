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


        [Test]
        public void SplitEmptyString()
        {
            "".SplitText().ShouldBeEmpty();
        }


        [Test]
        public void SplitSpaces()
        {
            "    ".SplitText().ShouldBeEmpty();
        }


        [Test]
        public void SplitOneWord()
        {
            "Foo".SplitText().ShouldBe(new[] { "Foo" });
        }


        [Test]
        public void SplitTwoWords()
        {
            "Homer Apu".SplitText().ShouldBe(new[] { "Homer", "Apu" });
        }


        [Test]
        public void SingleMiddleTabIsSplitAsWord()
        {
            "left\tright".SplitText().ShouldBe(new[] { "left", "\t", "right" });
        }


        [Test]
        public void DoubleMiddleTabsAreSplitAsWords()
        {
            "left\t\tright".SplitText().ShouldBe(new[] { "left", "\t", "\t", "right" });
        }
    }
}
