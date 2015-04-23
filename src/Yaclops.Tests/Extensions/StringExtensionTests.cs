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
            "foo".Decamel().ShouldBe("foo");
        }



        [Test]
        public void DecamelOneWordUpper()
        {
            "Foo".Decamel().ShouldBe("foo");
        }


        [Test]
        public void DecamelTwoWords()
        {
            "FooBar".Decamel().ShouldBe("foo bar");
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
