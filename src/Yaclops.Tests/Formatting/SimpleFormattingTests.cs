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


        [Test, Ignore("Get this working!")]
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



        private IList<string> Render(Document doc)
        {
            var console = new MockConsole();
            var formatter = new ConsoleFormatter(console);
            formatter.Format(doc);
            return console.Result;
        }
    }
}
