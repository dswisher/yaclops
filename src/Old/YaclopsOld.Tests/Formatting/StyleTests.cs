
using NUnit.Framework;
using Shouldly;
using Yaclops.Formatting;

namespace Yaclops.Tests.Formatting
{
    [TestFixture]
    public class StyleTests
    {

        [Test]
        public void CanSetStyleOnParagraph()
        {
            var doc = new Document();
            var para = doc.AddParagraph();

            var style = new StyleEx();

            style.SetIndent("p", 5);

            style.GetIndent(para).ShouldBe(5);
        }



        [Test]
        public void CanSetStyleOnBody()
        {
            var doc = new Document();

            var style = new StyleEx();

            style.SetIndent("body", 5);

            style.GetIndent(doc).ShouldBe(5);
        }



        [Test]
        public void CanInheritStyle()
        {
            var doc = new Document();
            var para = doc.AddParagraph();

            var style = new StyleEx();

            style.SetIndent("body", 5);

            style.GetIndent(para).ShouldBe(5);
        }


        // TODO - verify override - set indent on doc, then para, pull via para
    }
}

