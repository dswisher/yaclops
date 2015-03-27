using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using Yaclops.Parsing;

namespace Yaclops.Tests.Parsing
{
    [TestFixture]
    public class LexerTests
    {

        [Test]
        public void CanLexName()
        {
            var lexer = new Lexer("-flag");
            var token = lexer.Pop();
            token.Kind.ShouldBe(TokenKind.Name);
        }

    }
}
