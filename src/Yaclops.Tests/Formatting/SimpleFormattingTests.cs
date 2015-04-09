using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using Yaclops.Formatting;

namespace Yaclops.Tests.Formatting
{
    [TestFixture]
    public class SimpleFormattingTests
    {

        [Test]
        public void OneBlock()
        {
            var doc = new Document();
            doc.AddParagraph().AddBlock("Hello");

            var lines = Render(doc);

            lines.ShouldNotBeEmpty();
            lines[0].ShouldBe("Hello");
        }


        [Test]
        public void TwoBlocks()
        {
            var doc = new Document();
            var para = doc.AddParagraph();
            para.AddBlock("Hello");
            para.AddBlock("World!");

            var lines = Render(doc);

            lines.ShouldNotBeEmpty();
            lines[0].ShouldBe("Hello World!");
        }



        [Test]
        public void CanWrapSingleBlock()
        {
            var doc = new Document();
            doc.AddParagraph().AddBlock("Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
            //                           12345678901234567890

            // Set console width to 20 to force wrapping
            var lines = Render(doc, 20);

            lines[0].ShouldBe("Lorem ipsum dolor");
        }


        // TODO - test indent
        // TODO - test tab stops


        private IList<string> Render(Document doc, int width = 80)
        {
            var console = new MockConsole(width);
            var formatter = new ConsoleFormatter(console);
            formatter.Format(doc);
            return console.Result;
        }
    }
}
