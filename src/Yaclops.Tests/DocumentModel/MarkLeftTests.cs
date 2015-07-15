using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Shouldly;
using Yaclops.DocumentModel;

namespace Yaclops.Tests.DocumentModel
{
    [TestFixture]
    public class MarkLeftTests
    {

        [Test]
        public void NullContentReturnsEmpty()
        {
            var paras = MarkLeft.Parse(null);

            paras.ShouldBeEmpty();
        }



        [Test]
        public void EmptyContentReturnsEmpty()
        {
            var paras = MarkLeft.Parse(string.Empty);

            paras.ShouldBeEmpty();
        }



        [Test]
        public void SingleLineGivesOneParaOneSpan()
        {
            const string content = "This is a single line of text with no markup.";

            var paras = MarkLeft.Parse(content);

            var expected = new Paragraph();
            expected.AddSpan(content);

            paras.Dump().ShouldBe((new[] { expected }).Dump());
        }



        [Test]
        public void TwoLinesGivesOneParaOneSpan()
        {
            var paras = MarkLeft.Parse(@"
This is still one
span, but with newlines.
");

            var expected = new Paragraph();
            expected.AddSpan("This is still one span, but with newlines.");

            paras.Dump().ShouldBe((new[] { expected }).Dump());
        }



        [Test]
        public void BlankLineSeparatorGivesTwoParas()
        {
            var paras = MarkLeft.Parse(@"
One para.

Two para.
");

            var expected1 = new Paragraph();
            expected1.AddSpan("One para.");

            var expected2 = new Paragraph();
            expected2.AddSpan("Two para.");

            paras.Dump().ShouldBe((new[] { expected1, expected2 }).Dump());
        }
    }



    internal static class DocExtensions
    {
        public static string Dump(this IEnumerable<Paragraph> paras)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var para in paras.Select((x, i) => new { Para = x, Index = i }))
            {
                if (builder.Length > 0)
                {
                    builder.AppendLine();
                }

                builder.AppendFormat("P{0}", para.Index);

                foreach (var span in para.Para.Spans.Select((x, i) => new { Span = x, Index = i }))
                {
                    builder.AppendFormat(" [S{0}:{1}]", span.Index, span.Span.Text);
                }
            }

            return builder.ToString();
        }
    }
}
