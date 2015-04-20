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
            //                                             12345678901234567890
            //                                                       12345678901234567890
            //                                                                   12345678901234567890

            // Set console width to 20 to force wrapping
            var lines = Render(doc, 20);

            lines[0].ShouldBe("Lorem ipsum dolor");
            lines[1].ShouldBe("sit amet,");
            lines[2].ShouldBe("consectetur");
            lines[3].ShouldBe("adipiscing elit.");
        }


        [Test]
        public void CanWrapMultipleBlocks()
        {
            var doc = new Document();
            var para = doc.AddParagraph();
            para.AddBlock("Lorem");
            para.AddBlock("ipsum dolor");
            para.AddBlock("sit amet, consectetur adipiscing elit.");
            para.AddBlock("adipiscing elit.");

            // Set console width to 20 to force wrapping
            var lines = Render(doc, 20);

            lines[0].ShouldBe("Lorem ipsum dolor");
            lines[1].ShouldBe("sit amet,");
            lines[2].ShouldBe("consectetur");
            lines[3].ShouldBe("adipiscing elit.");
        }


        [Test]
        public void CanIndentOneLine()
        {
            var doc = new Document();
            var para = doc.AddParagraph();
            para.AddBlock("Lorem");
            para.AddBlock("ipsum");

            para.Style.Indent = 4;

            // Set console width to 20 to force wrapping
            var lines = Render(doc, 20);

            lines[0].ShouldBe("    Lorem ipsum");
        }



        [Test]
        public void CanIndentMultipleLines()
        {
            var doc = new Document();
            var para = doc.AddParagraph();
            para.AddBlock("Lorem ipsum dolor sit amet, consectetur adipiscing elit.");

            para.Style.Indent = 4;

            // Set console width to 20 to force wrapping
            var lines = Render(doc, 20);

            lines[0].ShouldBe("    Lorem ipsum");
            lines[1].ShouldBe("    dolor sit amet,");
            lines[2].ShouldBe("    consectetur");
                            // 12345678901234567890
        }



        [Test, Ignore("Get this working!")]
        public void TabStopAtStartWorks()
        {
            var doc = new Document();
            var para = doc.AddParagraph();
            para.AddBlock("\tLorem ipsum");

            para.Style.Tabs = new[] { 3 };

            // Set console width to 20 to force wrapping
            var lines = Render(doc, 20);

            lines[0].ShouldBe("   Lorem ipsum");
        }


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
