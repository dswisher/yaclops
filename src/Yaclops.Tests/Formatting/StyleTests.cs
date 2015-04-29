
using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using Yaclops.Formatting;

namespace Yaclops.Tests.Formatting
{
    [TestFixture]
	public class StyleTests
	{

		public void CanSetStyle()
		{
            var doc = new Document();
            var para = doc.AddParagraph();

			var style = new StyleEx();

			// TODO - verify the style info
			false.ShouldBe(true);
		}

	}
}

