
using NUnit.Framework;
using Shouldly;
using Yaclops.Formatting;

namespace Yaclops.Tests.Formatting
{
    [TestFixture]
	public class StyleTests
	{

        [Test]
		public void CanSetStyle()
		{
            var doc = new Document();
            var para = doc.AddParagraph();

			var style = new StyleEx();

            style.SetIndent("p", 5);

            style.GetIndent(para).ShouldBe(5);
		}


        // TODO - verify inheritance - set indent on doc and pull via para
        // TODO - verify override - set indent on doc, then para, pull via para
	}
}

